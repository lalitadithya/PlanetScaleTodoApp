import React from 'react';
import { TextField, loadTheme, IconButton } from '@fluentui/react';

export const AddTodoItem : React.FunctionComponent = () => {
  
  return (
    <div className={"AddTodoItemContainer"} style={{paddingTop: 20}}>
      <TextField label="Add Todo Item:" underlined />
      <IconButton iconProps={{iconName:'Add'}} title="Add" ariaLabel="Add" />
    </div>
  )
}