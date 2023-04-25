/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./index.html', './public/**/*.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {},
    keyframes: {
      'fade-out': {
        from: { opacity: 1 },
        to: { opacity: 0 }
      },
      'fade-in': {
        from: { opacity: 0 },
        to: { opacity: 1 }
      }
    },
    animation: {
      'fade-out': 'fade-out 250ms ease-in-out',
      'fade-in': 'fade-in 250ms ease-in-out'
    },
    container: {
      center: true
    }
  },
  plugins: [require('@tailwindcss/forms')]
}
