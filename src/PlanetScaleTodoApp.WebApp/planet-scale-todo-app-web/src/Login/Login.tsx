import React from 'react'
import { useParams, withRouter, RouteComponentProps, Redirect } from 'react-router-dom';
import { Cookies } from "react-cookie";
import { DefaultButton, PrimaryButton } from '@fluentui/react';

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
        <div className="loginContainer">
          <div className="loginContent">
            <div className="loginHeader">
              <h1>Planet scale To-Do app</h1>
            </div>
            <div className="loginButtons">
                <PrimaryButton 
                  className="loginButton"
                  text='Login with Google'
                />
                <PrimaryButton 
                  className="loginButton"
                  text='Login with Microsoft'
                />
                <PrimaryButton 
                  className="loginButton"
                  text='Login with Facebook'
                />
                <PrimaryButton 
                  className="loginButton"
                  text='Login with Twitter'
                />
            </div>
          </div>
        </div>
      )
    }
  }
}

export default withRouter(Login)