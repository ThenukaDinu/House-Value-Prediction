const express = require('express');
const cors = require('cors');
const path = require('path');
const fs = require('fs');
require('dotenv').config();

const app = express();
app.use(cors());

// Serve static files
app.use(express.static(path.join(__dirname, 'public')));

// Set up the image directory
const DEFAULT_IMAGE_DIR = 'HouseImages';
const imageDir = path.resolve(process.env.IMAGE_DIR || DEFAULT_IMAGE_DIR);
try {
  if (!fs.existsSync(imageDir)) {
    fs.mkdirSync(imageDir, { recursive: true });
  }
} catch (err) {
  console.error(`Error creating image directory: ${err.message}`);
  process.exit(1);
}

// Serve images from the specified directory
app.use('/images', express.static(imageDir));

// Start the server
const port = process.env.APP_PORT || 3005;
app.listen(port, () => {
  console.log(`Server running on port ${port}`);
  console.log(`http://localhost:${port}`);
});

module.exports = app;
