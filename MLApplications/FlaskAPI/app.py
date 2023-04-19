import logging
from flask import Flask, request, jsonify
import pickle
from logging.handlers import TimedRotatingFileHandler
import os
from datetime import datetime
import pandas as pd
app = Flask(__name__)

if os.environ.get('FLASK_ENV') == 'production':
    log_dir = os.getenv('LOG_DIR', 'logs')
else:
    log_dir = 'C:\\Logs\\ML_Flask_API'

# setup logging
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

# Get the full path to the model file
model_file_path = os.path.join(
    os.getcwd(), 'Models', 'house_value_prediction_RF01.h5')

# Load the trained machine learning model
with open(model_file_path, 'rb') as f:
    model = pickle.load(f)


@app.route('/', methods=['GET'])
def get_request():
    app.logger.info('/ end point being called')
    return 'Prediction server is up and running...'


@app.route('/predict', methods=['POST'])
def predict():
    try:
        app.logger.info('/predict end point being called')

        # Extract the JSON payload from the request
        data = request.get_json()
        app.logger.info(f'input data to predict: {data}')

        # Convert the JSON payload to a pandas dataframe
        df = pd.DataFrame.from_dict(data)

        # Make predictions using the loaded machine learning model
        predictions = model.predict(df)

        # Convert the predictions to a list
        output = predictions.tolist()

        app.logger.info(f'predictions: {output}')

        # Return the predictions in JSON format
        return jsonify({'predictions': output})
    except Exception as e:
        app.logger.error(f'error: {e}')
        return jsonify({'error': str(e)})


if __name__ == '__main__':
    app.logger.info('Starting house value prediction ML API...')
    app.run(host="0.0.0.0", port=5000, debug=True, use_reloader=True)
