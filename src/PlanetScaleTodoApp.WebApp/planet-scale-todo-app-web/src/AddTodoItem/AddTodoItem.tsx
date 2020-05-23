import React from 'react';
import { TextField, IconButton } from '@fluentui/react';

type AddTodoItemProps = { onAddCallback: (itemName: string) => void }
type AddTodoItemState = { todoItem: string }

class AddTodoItem extends React.Component<AddTodoItemProps, AddTodoItemState> {
  
  constructor(props: AddTodoItemProps) {
    super(props)

    this.state = {
      todoItem: ""
    }
  }

  private addTodoItem() {
    this.props.onAddCallback(this.state.todoItem)
    this.setState({
      todoItem: ""
    })
  }

  private todoItemChanged(newValue?: string) {
    this.setState({
      todoItem: newValue === undefined ? "" : newValue
    })
  }

  private onKeyDown(kv : React.KeyboardEvent<{}>) {
    if(kv.key === "Enter") {
      this.addTodoItem()
    }
  }

  public render() {
    return (
      <div className={"AddTodoItemContainer"} style={{paddingTop: 20}}>
        <TextField 
          label="Add Todo Item:" 
          value={this.state.todoItem} 
          underlined 
          onChange={(ev, newValue) => this.todoItemChanged(newValue)} 
          onKeyDown={(kv) => this.onKeyDown(kv)} 
        />
        <IconButton iconProps={{iconName:'Add'}} title="Add" ariaLabel="Add" onClick={() => this.addTodoItem()}/>
      </div>
    )
  }
}

export default AddTodoItem