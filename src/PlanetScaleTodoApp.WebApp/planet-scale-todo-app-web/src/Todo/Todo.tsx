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

  public componentDidMount(){
    fetch("https://fa-todoapp.azurewebsites.net/api/GetTodoItems?code=yv2w7jd11VASHG8gKmVIfX9q/BC3zfTf/WXkvoIrfTZtDmGrfQOHjQ==", {
      credentials: "include",
      method: "get"
    }).then((response) => {
      response.json().then((data) => {
        console.log(data)
        let dataItems: ITodoItem[] = []
        data.forEach((element: any) => {
         dataItems.push({
           id:element.Id,
           isCompleted: element.IsCompleted,
           item:element.Item
         })
        });
        this.setState({
          todoItems: dataItems
        })
      })
    })
  }

  private addTodoItem(itemName: string) {
    let data = JSON.stringify({
      Item: itemName,
      IsCompleted: false
    })
    fetch("https://fa-todoapp.azurewebsites.net/api/AddTodoItem?code=EfzaJ0SX7VLfUB1yGDmkgP6Q5Hr3bKwwUkNHtwwYTyD0cwOLAqtFqQ==", {
      method: "post",
      body: data,
      credentials: "include",
      headers: { 'Content-Type': 'application/json'}
    }).then(response => {
      response.text().then(id => {
        this.setState({
          todoItems: this.state.todoItems.concat({
              id: id,
              isCompleted: false,
              item: itemName
            } 
          )
        })
      })
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