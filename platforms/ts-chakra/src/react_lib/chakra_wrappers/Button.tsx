import * as React from "react";
import {StyleSheet, css} from "aphrodite";
import { useNavigate } from "react-router-dom";

// import { Button as ChakraButton, Link } from '@chakra-ui/react'

type Props = {
  readonly children?: React.JSX.Element | string,
  readonly label?: string,
  readonly onClick?: () => unknown,
  readonly url?: string,
  readonly disabled?: boolean,
  readonly style?: "normal" | "link",
  readonly intent?: "default" | "save-changes"
};
export default function Button(props: Props): React.JSX.Element {
  let {
    children, 
    label="Click", 
    onClick, 
    url, 
    disabled=false, 
    style="normal",
    intent="default"
  } = props;
  const navigation = useNavigate();

  children = children || label;

  function navigateToUrl() {
    if (url != null)
      navigation(url);
  }

  const myOnClick = url == null ? onClick : navigateToUrl;
  
  var myStyles;
  if (style == "normal") {
    myStyles = [styles.button];
    if (intent == "save-changes")
      myStyles.push(styles.buttonSaveChanges);
  } else {
    myStyles = [styles.link];
  }

  return (
      <button 
        onClick={myOnClick}
        disabled={disabled}
        className={css(myStyles)}
      >
        {children}
      </button>
  );
}

const styles = StyleSheet.create({
    button: {
      backgroundColor: '#4CAF50',
      border: 'none',
      color: 'white', /* White text */
      padding: '5px 10px',
      textAlign: 'center',
      textDecoration: 'none',
      display: 'inline-block',
      fontSize: '16px',
      margin: '4px 2px',
      cursor: 'pointer',
      borderRadius: '8px',
      transition: 'background-color 0.3s ease', /* Smooth transition for hover effect */
      ':hover': {
        backgroundColor: '#259039', /* Darker shade of green */
      }
    },
    link: {
      background: 'none',
      border: 'none',
      padding: 0,
      cursor: 'pointer',
      color: '#0000EE',
      textDecoration: 'underline',
      ':hover': {
        textDecoration: 'none',
      }
    },
    buttonSaveChanges: {
      backgroundColor: '#1E90FF',
      ':hover': {
        backgroundColor: '#0000FF', /* Darker shade of green */
      }
    }
  });
