# Use an official Python runtime as a parent image
FROM python:3.8-slim-buster

# Set the working directory to /app
WORKDIR /app

# Copy the requirements file into the container at /app
COPY requirements.txt .

# Install any needed packages specified in requirements.txt
RUN pip install --trusted-host pypi.python.org -r requirements.txt

# Copy the rest of the application code into the container at /app
COPY app.py .

# Set up logging directory
RUN mkdir -p /var/log/ML_Flask_API

# Set environment variables
ENV FLASK_APP=app.py
ENV FLASK_ENV=production
ENV LOG_DIR=/app/logs

# Expose port 5000 for the Flask app
EXPOSE 5000

# Run the command to start the Flask application
CMD ["flask", "run", "--host=0.0.0.0"]