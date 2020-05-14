import React, { MouseEvent } from 'react'
import { useParams, withRouter, RouteComponentProps, Redirect, useHistory } from 'react-router-dom';
import { Cookies } from "react-cookie";
import { DefaultButton, PrimaryButton, Button, BaseButton } from '@fluentui/react';

interface RouteParams {
  token: string
}

class Login extends React.Component<RouteComponentProps<{}>, {}> {

  constructor(props: RouteComponentProps<{}>){
    super(props)
    
    this.loginWithGoogle = this.loginWithGoogle.bind(this)
  }

  private loginWithGoogle(e: React.MouseEvent<HTMLButtonElement | HTMLAnchorElement | HTMLDivElement | BaseButton | Button | HTMLSpanElement, globalThis.MouseEvent>) {
    e.preventDefault();
    this.props.history.listen((location, action) => {
      console.log(location)
      console.log(action)
    })
    console.log(this.props);
    
    //let w = window.open("https://fa-todoapp-dev.azurewebsites.net/.auth/login/google?post_login_redirect_url=http%3A%2F%2Flocalhost%3A3000%2F")
    window.location.href = "https://fa-todoapp.azurewebsites.net/.auth/login/google?post_login_redirect_url=https%3A%2F%2Ffa-todoapp.azurewebsites.net%2Fapi%2FGetUserId%3Fcode%3Dk4gQl4AgCocvaP44zyqlC6KOcHYg7Z3yOd9wLwcXePwazYEtwnqbTw%3D%3D"
  }

  public render(){
    let  c= new Cookies();
    console.log(this.props)
    if(c.get("authToken") !== undefined) {
      return (
        <Redirect to='/home'/>
      )
    } else if (this.props.location.search !== "") {
      console.log(this.props.location.search.substring(7))
      let authenticationToken = decodeURIComponent(this.props.location.search.substring(7))
      console.log(authenticationToken)
      c.set("AppServiceAuthSession", authenticationToken, {
        domain:"https://fa-todoapp.azurewebsites.net/",
        httpOnly: true,
        path:"/"
      })
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
                  onClick={(e) => this.loginWithGoogle(e)}
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