import { createMuiTheme } from '@material-ui/core';
import { green, pink } from '@material-ui/core/colors';

const pinkTheme = createMuiTheme({
  palette: {
    primary: pink,
    secondary: green,
  },
});

export default pinkTheme;
