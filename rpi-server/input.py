from flask import Flask
from flask import render_template
import RPi.GPIO as GPIO
import pigpio
import time
from markupsafe import escape
PIN_SERVO = 21
PIN_ENA = 2
PIN_IN1 = 3
PIN_IN2 = 4
PIN_IN3 = 17
PIN_IN4 = 27
PIN_ENB = 22
PIN_R = 6
PIN_G = 5
PIN_B = 13
#PIN_TRUBA=
PIN_LED = 19
gpio=pigpio.pi()
gpio.set_mode(PIN_SERVO, pigpio.OUTPUT)
gpio.set_mode(PIN_ENA, pigpio.OUTPUT)
gpio.set_mode(PIN_ENB, pigpio.OUTPUT)
gpio.set_mode(PIN_IN1, pigpio.OUTPUT)
gpio.set_mode(PIN_IN2, pigpio.OUTPUT)
gpio.set_mode(PIN_IN3, pigpio.OUTPUT)
gpio.set_mode(PIN_IN4, pigpio.OUTPUT)
gpio.set_mode(PIN_LED, pigpio.OUTPUT)
gpio.set_mode(PIN_R, pigpio.OUTPUT)
gpio.set_mode(PIN_G, pigpio.OUTPUT)
gpio.set_mode(PIN_B, pigpio.OUTPUT)
gpio.set_PWM_frequency(PIN_SERVO, 50)
gpio.set_PWM_frequency(PIN_ENA, 50)
gpio.set_PWM_frequency(PIN_ENB, 50)
gpio.set_PWM_frequency(PIN_R, 500)
gpio.set_PWM_frequency(PIN_G, 500)
gpio.set_PWM_frequency(PIN_B, 500)
gpio.set_servo_pulsewidth(PIN_SERVO, 1500)
gpio.set_PWM_dutycycle(PIN_ENA, 255)
gpio.set_PWM_dutycycle(PIN_ENB, 255)
gpio.set_PWM_dutycycle(PIN_R, 0)
gpio.set_PWM_dutycycle(PIN_G, 0)
gpio.set_PWM_dutycycle(PIN_B, 0)
PAGE="""\
<html>
<body style="background-color:black;">
    <div>
        <img src="https://192.168.1.67:5000/stream.mjpg" width="1296" height="730"/>
    </div>
</body>
</html>
"""

app = Flask(__name__)

@app.route('/')
def index():
	return render_template("index.html")

@app.route('/a')
def left():
	gpio.set_servo_pulsewidth(PIN_SERVO, 1100);
	return render_template("index.html")

@app.route('/A')
def left_stop():
	gpio.set_servo_pulsewidth(PIN_SERVO, 1425);
	return render_template("index.html")

@app.route('/d')
def right():
	gpio.set_servo_pulsewidth(PIN_SERVO, 1750);
	return render_template("index.html")

@app.route('/D')
def right_stop():
	gpio.set_servo_pulsewidth(PIN_SERVO, 1425);
	return render_template("index.html")

@app.route('/w')
def forward():
	gpio.write(PIN_IN1,0)
	gpio.write(PIN_IN3,0)
	gpio.write(PIN_IN2,1)
	gpio.write(PIN_IN4,1)
	return render_template("index.html")

@app.route('/W')
def forward_stop():
	gpio.write(PIN_IN1,0)
	gpio.write(PIN_IN2,0)
	gpio.write(PIN_IN3,0)
	gpio.write(PIN_IN4,0)
	return render_template("index.html")

@app.route('/s')
def backward():
	gpio.write(PIN_IN2,0)
	gpio.write(PIN_IN4,0)
	gpio.write(PIN_IN1,1)
	gpio.write(PIN_IN3,1)
	return render_template("index.html")

@app.route('/S')
def backward_stop():
	gpio.write(PIN_IN1,0)
	gpio.write(PIN_IN2,0)
	gpio.write(PIN_IN3,0)
	gpio.write(PIN_IN4,0)
	return render_template("index.html")

@app.route("/led")
def led_on():
	gpio.write(PIN_LED,1)
	return render_template("index.html")

@app.route("/LED")
def led_off():
	gpio.write(PIN_LED,0)
	return render_template("index.html")

@app.route('/speed/<int:spd>')
def setupspeed(spd):
	gpio.set_PWM_dutycycle(PIN_ENA,spd)
	gpio.set_PWM_dutycycle(PIN_ENB,spd)
	return render_template("index.html")

@app.route('/frequency/<int:freq>')
def setupfrequency(freq):
	gpio.set_PWM_frequency(PIN_ENA,freq)
	gpio.set_PWM_frequency(PIN_ENB,freq)
	return render_template("index.html")


@app.route('/r/<int:red>')
def rgb_r(red):
	gpio.set_PWM_dutycycle(PIN_R, red)
	return render_template("index.html")

@app.route('/g/<int:green>')
def rgb_g(green):
	gpio.set_PWM_dutycycle(PIN_G, green)
	return render_template("index.html")

@app.route('/b/<int:blue>')
def rgb_b(blue):
	gpio.set_PWM_dutycycle(PIN_B, blue)
	return render_template("index.html")

if __name__ == '__main__':
	app.run(debug=False, port=8000, host='0.0.0.0')
