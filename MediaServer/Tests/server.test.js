const request = require('supertest');
const express = require('express');
const fs = require('fs');
const path = require('path');
const app = require('../server');

describe('Image server', () => {
  const nonExistentImagePath = path.join(
    __dirname,
    '..',
    'Tests',
    'nonexistent.jpg'
  );

  describe('GET /images/:filename', () => {
    it('should return 404 for a non-existent image file', async () => {
      const response = await request(app).get(
        `/images/${path.basename(nonExistentImagePath)}`
      );
      expect(response.statusCode).toBe(404);
    });
  });
});
