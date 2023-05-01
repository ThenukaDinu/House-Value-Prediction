<script setup></script>

<template>
  <div class="home_page">
    <video poster="welcome-image.png" playsinline autoplay muted loop>
      <source src="background_video_1.mp4" type="video/mp4" />
    </video>
    <div id="fashion">
      <h2>Discover the True Value of Your House with Our Accurate Prediction Tool</h2>
    </div>
  </div>
</template>

<style scoped lang="scss">
.home_page {
  display: flex;
  justify-content: center;
  align-items: center;
  height: calc(100vh - 54px);
}

@font-face {
  font-family: Moderne Sans;
  src: url(https://s3-us-west-2.amazonaws.com/s.cdpn.io/4273/moderne_sans.woff2);
}
* {
  box-sizing: border-box;
}
body {
  margin: 0;
  background: #000;
  background-size: cover;
}
video {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}
div#fashion {
  min-height: calc(100vh - 54px);
  display: flex;
  align-items: center;
  justify-content: center;
}

h1 {
  font-family: Moderne Sans, sans-serif;
  text-align: center;
  font-size: 2rem;
  width: 100%;
  letter-spacing: 0.5rem;
}
nav a {
  text-decoration: none;
  color: inherit;
  padding: 1rem;
}
h2 {
  font-family: Century Schoolbook, Century Schoolbook L, Georgia, serif;
  font-size: 8vmin;
  text-align: center;
  margin: 2rem 3rem 0;
  mix-blend-mode: overlay;
  color: #f3f3f3;
  font-weight: 100;
}
</style>
