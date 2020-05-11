import React from 'react'
import AddTodoItem from '../AddTodoItem/AddTodoItem'
import ViewTodoItems from '../ViewTodoItems/ViewTodoItems'
import ITodoItem from '../models/TodoItem'

type TodoItemsState = {
  todoItems: ITodoItem[]
}

class Todo extends React.Component<{}, TodoItemsState> {

  constructor(props: {}) {
    super(props)

    this.state = {
      todoItems: []
    }

    this.addTodoItem = this.addTodoItem.bind(this)
  }

  private addTodoItem(itemName: string) {
    console.log(this.state)
    this.setState({
      todoItems: this.state.todoItems.concat({
          id: (new Date()).getTime() +"",
          isCompleted: false,
          item: itemName
        } 
      )
    })
  }

  public render() {
    return (
      <div className={'container'}>
        <AddTodoItem onAddCallback={this.addTodoItem} />
        <ViewTodoItems todoItems={this.state.todoItems}/>
      </div>
    )
  }
}


export default Todo