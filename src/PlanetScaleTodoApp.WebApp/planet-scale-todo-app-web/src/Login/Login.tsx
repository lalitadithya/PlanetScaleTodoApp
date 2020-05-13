import React from 'react'
import { useParams, withRouter, RouteComponentProps, Redirect } from 'react-router-dom';
import { Cookies } from "react-cookie";

interface RouteParams {
  token: string
}

class Login extends React.Component<RouteComponentProps<RouteParams>, {}> {
  public render(){
    let  c= new Cookies();
    if(c.get("authToken") !== undefined) {
      return (
        <Redirect to='/home'/>
      )
    } else if (this.props.location.hash !== "") {
      let authenticationToken = JSON.parse(decodeURIComponent(this.props.location.hash.substring(7))).authenticationToken
      console.log(authenticationToken)
      c.set("authToken", authenticationToken)
      return (
        <Redirect to='/home'/>
      )
    } else {
      return(
        <div>
          <h1> Welcome </h1>
        </div>
      )
    }
  }
}

export default withRouter(Login)