import * as React from "react";
import {StyleSheet, css} from "aphrodite";
import { useNavigate } from "react-router-dom";

// import { Button as ChakraButton, Link } from '@chakra-ui/react'

type Props = {
  readonly label: string,
  readonly onClick?: () => unknown,
  readonly url?: string,
  readonly disabled?: boolean,
  readonly style?: "normal" | "link"
};
export default function Button(props: Props): React.JSX.Element {
  const {label, onClick, url, disabled=false, style='normal'} = props;

  const navigation = useNavigate();

  function navigateToUrl() {
    if (url != null)
      navigation(url);
  }

  const myOnClick = url == null ? onClick : navigateToUrl;
  const myStyle = style == 'normal' ? styles.button : styles.link;
  
  return (
      <button 
        onClick={myOnClick}
        disabled={disabled}
        className={css(myStyle)}
      >
        {label}
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
  });
