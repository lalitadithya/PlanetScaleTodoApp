import React, { EventHandler } from 'react';
import ITodoItem from '../models/TodoItem'
import { DetailsList, IColumn, SelectionMode, Checkbox, IDetailsListProps, IDetailsRowStyles, DetailsRow } from '@fluentui/react';

type TodoItemsListState = {
  todoItems: ITodoItem[]
  columns: IColumn[]
}

type TodoItemsListProps = {
  todoItems: ITodoItem[]
}

class ViewTodoItems extends React.Component<TodoItemsListProps, TodoItemsListState> {

  constructor(props: TodoItemsListProps) {
    super(props)
    this.state = {
      todoItems: this.props.todoItems,
      columns: [
        {
          key: "checkbox",
          name:"Is Completed",
          minWidth: 16,
          maxWidth: 16,
          isIconOnly: true,
          iconName:"CheckboxComposite",
          onRender: (item: ITodoItem) => {
            return (
              <Checkbox defaultChecked={item.isCompleted} onChange={(e, c) => this.ItemChanged(item)}/>
            )
          }
        },
        {
          key: "item",
          name: "Todo Item",
          fieldName: "item",
          minWidth: 100,
          onRender: (item: ITodoItem) => {
            if(item.isCompleted) {
              return(
                <div style={{textDecoration:"line-through"}}>
                  {item.item}
                </div>
              )
            } else {
              return(
                <div>
                  {item.item}
                </div>
              )
            }
          }
        }
      ]
    }
  }

  private ItemChanged(item: ITodoItem): void {
    const todoItems = [...this.state.todoItems]
    for(let i = 0; i < todoItems.length; i++) {
      if(todoItems[i].id == item.id) {
        fetch("https://fa-todoapp.azurewebsites.net/api/ToggleTodoItem?code=pz6oUayhTA1DdCF5JjEswjkjMo5qBp7gf4LG9/djVpG0flYMmghLKw==&todoItemId="+item.id, {
          credentials: "include",
          method: "put"
        })
        todoItems[i].isCompleted = !todoItems[i].isCompleted
      }
    }

    this.setState({
      todoItems: todoItems
    })
  }

  public componentDidUpdate(prevProps: TodoItemsListProps, prevState: TodoItemsListState) {
    if(prevProps.todoItems.length !== this.props.todoItems.length) {
      this.setState({
        todoItems: [... this.props.todoItems]
      })
    }
  }

  public render() {
    const {todoItems} = this.state
    return (
      <DetailsList 
        items={todoItems}
        columns={this.state.columns}
        selectionMode={SelectionMode.none}
        />
    )
  }
}

export default ViewTodoItems