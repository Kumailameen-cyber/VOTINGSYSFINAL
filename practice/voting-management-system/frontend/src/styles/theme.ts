export const theme = {
  colors: {
    primary: '#4CAF50',
    secondary: '#FF5722',
    background: '#F5F5F5',
    text: '#212121',
    border: '#E0E0E0',
  },
  typography: {
    fontFamily: '"Roboto", sans-serif',
    fontSize: '16px',
    fontWeightRegular: 400,
    fontWeightBold: 700,
  },
  spacing: (factor) => `${0.25 * factor}rem`,
  breakpoints: {
    mobile: '480px',
    tablet: '768px',
    desktop: '1024px',
  },
};