import React, { EventHandler } from 'react';
import ITodoItem from '../models/TodoItem'
import { DetailsList, IColumn, SelectionMode, Checkbox, IDetailsListProps, IDetailsRowStyles, DetailsRow } from '@fluentui/react';

type TodoItemsListState = {
  todoItems: ITodoItem[]
  columns: IColumn[]
}

class ViewTodoItems extends React.Component<{}, TodoItemsListState> {

  constructor(props: {}) {
    super(props)
    this.state = {
      todoItems: [ 
        {
          id: "1",
          isCompleted: false,
          item: "Item 1"
        },
        {
          id: "2",
          isCompleted: false,
          item: "Item 2"
        },
        {
          id: "3",
          isCompleted: false,
          item: "Item 3"
        },
        {
          id: "4",
          isCompleted: true,
          item: "Item 4"
        }
      ],
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
        todoItems[i].isCompleted = !todoItems[i].isCompleted
      }
    }

    this.setState({
      todoItems: todoItems
    })
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