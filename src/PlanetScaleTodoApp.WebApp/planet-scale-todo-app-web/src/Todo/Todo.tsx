import React from 'react'
import AddTodoItem from '../AddTodoItem/AddTodoItem'
import ViewTodoItems from '../ViewTodoItems/ViewTodoItems'

class Todo extends React.Component<{}, {}> {

  private addTodoItem(itemName: string) {
    console.log(itemName)
  }

  public render() {
    return (
      <div className={'container'}>
        <AddTodoItem onAddCallback={this.addTodoItem} />
        <ViewTodoItems />
      </div>
    )
  }
}

export default Todo