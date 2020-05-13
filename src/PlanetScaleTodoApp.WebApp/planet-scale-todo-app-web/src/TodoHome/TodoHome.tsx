import React from 'react'
import { TopNav } from '../_component/TopNav/TopNav'
import Todo from '../Todo/Todo'

export default class TodoHome extends React.Component<{}, {}> {
  public render(){
    return(
      <div>
        <TopNav />
        <Todo />
      </div>
    )
  }
}