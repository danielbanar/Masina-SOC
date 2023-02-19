#!/usr/bin/python3
import io
import picamera
import logging
import socketserver
from evdev import InputDevice
from threading import Condition
from http import server
import threading, os, signal
import subprocess
from subprocess import check_call, call
import sys

PAGE="""\
<html>
<body style="background-color:black;">
    <div>
        <img src="stream.mjpg" width="1296" height="730"/>
    </div>
</body>
</html>
"""

class StreamingOutput(object):
    def __init__(self):
        self.frame = None
        self.buffer = io.BytesIO()
        self.condition = Condition()

    def write(self, buf):
        if buf.startswith(b'\xff\xd8'):
            self.buffer.truncate()
            with self.condition:
                self.frame = self.buffer.getvalue()
                self.condition.notify_all()
            self.buffer.seek(0)
        return self.buffer.write(buf)

class StreamingHandler(server.BaseHTTPRequestHandler):
    def do_GET(self):
        if self.path == '/':
            self.send_response(301)
            self.send_header('Location', '/index.html')
            self.end_headers()
        elif self.path == '/index.html':
            content = PAGE.encode('utf-8')
            self.send_response(200)
            self.send_header('Content-Type', 'text/html')
            self.send_header('Content-Length', len(content))
            self.end_headers()
            self.wfile.write(content)
        elif self.path == '/stream.mjpg':
            self.send_response(200)
            self.send_header('Age', 0)
            self.send_header('Cache-Control', 'no-cache, private')
            self.send_header('Pragma', 'no-cache')
            self.send_header('Content-Type', 'multipart/x-mixed-replace; boundary=FRAME')
            self.end_headers()
            try:
                while True:
                    with output.condition:
                        output.condition.wait()
                        frame = output.frame
                    self.wfile.write(b'--FRAME\r\n')
                    self.send_header('Content-Type', 'image/jpeg')
                    self.send_header('Content-Length', len(frame))
                    self.end_headers()
                    self.wfile.write(frame)
                    self.wfile.write(b'\r\n')
            except Exception as e:
                logging.warning(
                    'Removed streaming client %s: %s',
                    self.client_address, str(e))
        else:
            self.send_error(404)
            self.end_headers()

class StreamingServer(socketserver.ThreadingMixIn, server.HTTPServer):
    allow_reuse_address = True
    daemon_threads = True
#2592 × 1944 @ 15fps
with picamera.PiCamera(resolution='1296x730', framerate=30) as camera:
    camera.rotation = 180
    output = StreamingOutput()
    camera.start_recording(output, format='mjpeg')
    try:
        address = ('', 5000)
        server = StreamingServer(address, StreamingHandler)
        print("Streaming.")
        server.serve_forever()

    finally:
        print("ERROR: Stream not able to run. Stream ended.")
        camera.stop_recording()

# print in the command line instead of file's console
if __name__ == '__main__':
    main()
