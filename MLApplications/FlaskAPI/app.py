import logging as logger
from flask import Flask, Request
app = Flask(__name__)


@app.route('/', methods=['GET'])
def get_request():
    return 'Prediction server is up and running...'


@app.route('/get-value-prediction', methods=['POST'])
def post_request():
    data = request.get_json()
    return f'This is a POST request. You sent: {data}'


if __name__ == '__main__':
    app.run()
