import React from 'react';
import { CommandBar, ICommandBarItemProps } from 'office-ui-fabric-react/lib/CommandBar';
import { IButtonProps } from 'office-ui-fabric-react/lib/Button';
import { createTheme } from '@fluentui/react'

const overflowProps: IButtonProps = { ariaLabel: 'More commands' };

const theme = createTheme({
  palette: {
    themePrimary: '#ffffff',
    themeLighterAlt: '#767676',
    themeLighter: '#a6a6a6',
    themeLight: '#c8c8c8',
    themeTertiary: '#d0d0d0',
    themeSecondary: '#dadada',
    themeDarkAlt: '#eaeaea',
    themeDark: '#f4f4f4',
    themeDarker: '#f8f8f8',
    neutralLighterAlt: '#282828',
    neutralLighter: '#313131',
    neutralLight: '#3f3f3f',
    neutralQuaternaryAlt: '#484848',
    neutralQuaternary: '#4f4f4f',
    neutralTertiaryAlt: '#6d6d6d',
    neutralTertiary: '#c8c8c8',
    neutralSecondary: '#d0d0d0',
    neutralPrimaryAlt: '#dadada',
    neutralPrimary: '#ffffff',
    neutralDark: '#f4f4f4',
    black: '#f8f8f8',
    white: '#1f1f1f',
  }
})

export const TopNav : React.FunctionComponent = () => {
  return (
    <div>
      <CommandBar
        items={_items}
        overflowItems={_overflowItems}
        overflowButtonProps={overflowProps}
        farItems={_farItems}
        theme={theme}
        ariaLabel="Use left and right arrow keys to navigate between commands"
      />
    </div>
  )
}

const _items: ICommandBarItemProps[] = [
  {
    key: 'home',
    text: 'Planet Scale Todo App',
    cacheKey: 'myCacheKey',
    theme: theme,
    buttonStyles:{
      textContainer: {
        fontWeight:"bolder",
        fontSize:20
      }
    }
  }
];

const _overflowItems: ICommandBarItemProps[] = [
];

const _farItems: ICommandBarItemProps[] = [
  {
    key: 'info',
    text: 'Info',
    ariaLabel: 'Info',
    iconOnly: true,
    iconProps: { 
      iconName: 'Info',
    },
    onClick: () => console.log('Info'),
    theme: theme
  },
  {
    key: 'SignOut',
    text: 'Sign Out',
    ariaLabel: 'Sign Out',
    iconOnly: true,
    iconProps: { 
      iconName: 'SignOut',
    },
    onClick: () => console.log('Signing out'),
    theme: theme
  }
];