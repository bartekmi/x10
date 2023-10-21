import * as React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

type Props = {
  readonly rootComponent: React.JSX.Element,
  readonly children: React.JSX.Element | React.JSX.Element[]
};
export default function SpaContent(props: Props): React.JSX.Element {
  const {rootComponent, children} = props;

  return (
    <Router>
      <Routes>
        <Route path='/' element={rootComponent}/>
        {children}
      </Routes>
    </Router>
  )
}

// Add History
// TODO: See https://www.geeksforgeeks.org/reactjs-usenavigate-hook/