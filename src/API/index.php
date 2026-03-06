<?php

// Store this more securely
$EXPECTED_SECRET = 'secret123';

$OUTPUT_DIR = __DIR__ . '/../logs';

function respond(int $code, string $message): never {
    http_response_code($code);
    header('Content-Type: application/json');
    echo json_encode(['message' => $message]);
    exit;
}

// Only accept POST requests
if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    respond(405, 'Method Not Allowed');
}

// Parse JSON body
$body = json_decode(file_get_contents('php://input'), true);
if (!$body) {
    respond(400, 'Invalid JSON');
}

// Validate required fields
foreach (['ComputerId', 'Secret', 'Day', 'Hex'] as $field) {
    if (empty($body[$field])) {
        respond(400, "Missing field: $field");
    }
}

// Verify password
if (!hash_equals($EXPECTED_SECRET, $body['Secret'])) {
    respond(403, 'Forbidden');
}

// Validate date format (YYYY-MM-DD)
if (!preg_match('/^\d{4}-\d{2}-\d{2}$/', $body['Day'])) {
    respond(400, 'Invalid Day format');
}

// Sanitize ComputerId to safe filename characters
$computerId = preg_replace('/[^a-zA-Z0-9_\-]/', '_', $body['ComputerId']);
$day        = $body['Day'];
$activity   = $body['Hex'];

// Create output directory if needed
if (!is_dir($OUTPUT_DIR)) {
    mkdir($OUTPUT_DIR, 0750, true);
}

// Write activity to file: activity_logs/2026-03-05_personal-laptop.txt
$filename = "$OUTPUT_DIR/{$day}_{$computerId}.txt";
$result = file_put_contents($filename, $activity, LOCK_EX);

if ($result === false) {
    respond(500, 'Failed to write file');
}

// Return a unique response each time
respond(200, 'Logged-' . bin2hex(random_bytes(8)));