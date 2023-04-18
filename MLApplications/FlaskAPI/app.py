import logging
from flask import Flask, Request
from logging.handlers import TimedRotatingFileHandler
import os
from datetime import datetime
app = Flask(__name__)

if os.environ.get('FLASK_ENV') == 'production':
    log_dir = os.getenv('LOG_DIR', 'logs')
else:
    log_dir = 'C:\\Logs\\ML_Flask_API'

if not os.path.exists(log_dir):
    os.makedirs(log_dir)

log_file = os.path.join(log_dir, 'log')
handler = TimedRotatingFileHandler(
    log_file + datetime.now().strftime('%Y%m%d') + '.log',
    when='midnight', interval=1, backupCount=7)

formatter = logging.Formatter(
    f'%(asctime)s %(levelname)s %(name)s %(threadName)s : %(message)s')
handler.setFormatter(formatter)

logger = logging.getLogger()
logger.addHandler(handler)
logger.setLevel(logging.DEBUG)


@app.route('/', methods=['GET'])
def get_request():
    app.logger.info('/ end point being called')
    return 'Prediction server is up and running...'


@app.route('/get-value-prediction', methods=['POST'])
def post_request():
    app.logger.info('/get-value-prediction end point being called')
    data = request.get_json()
    return f'This is a POST request. You sent: {data}'


if __name__ == '__main__':
    app.logger.info('Starting house value prediction ML API...')
    app.run(host="0.0.0.0", port=5000, debug=True, use_reloader=True)
