from app import app
from flask import json


def test_root():
    with app.test_client() as client:
        response = client.get('/')
        assert response.status_code == 200
        assert response.data == b'Prediction server is up and running...'


def test_predict():
    with app.test_client() as client:
        payload = [{
            "OverallQual": 8,
            "YearBuilt": 2005,
            "YearRemodAdd": 2005,
            "TotalBsmtSF": 856,
            "1stFlrSF": 856,
            "GrLivArea": 1710,
            "FullBath": 3,
            "TotRmsAbvGrd": 5,
            "GarageCars": 3,
            "GarageArea": 548
        },
            {
            "OverallQual": 6,
            "YearBuilt": 1976,
            "YearRemodAdd": 1976,
            "TotalBsmtSF": 1262,
            "1stFlrSF": 1262,
            "GrLivArea": 1262,
            "FullBath": 2,
            "TotRmsAbvGrd": 6,
            "GarageCars": 2,
            "GarageArea": 460
        }]
        response = client.post(
            '/predict', data=json.dumps(payload), content_type='application/json')
        data = json.loads(response.data)
        assert response.status_code == 200
        assert 'predictions' in data
