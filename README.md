# House-Value-Prediction

## identity_server_url: https://localhost:44342/

## house_manage_api_url: https://localhost:44341/

## rabbitMq interface: http://localhost:15672/

## ml_flask_api: http://localhost:44343/

## webApp URL: http://localhost:44344/

## docker commands for ml_flask_api:

<br>

`docker build -t ml_flask_api .`

`docker run -p 44343:5000 -v /var/log/ML_Flask_API:/app/logs ml_flask_api`
