﻿import * as React from 'react'
import { Tab, Tabs }from 'react-bootstrap'
import { classes } from '../../../Framework/Signum.React/Scripts/Globals'
import { FormGroup, FormControlStatic, EntityComponent, ValueLine, ValueLineType, EntityLine, EntityCombo, EntityDetail, EntityList, EntityRepeater, EntityFrame, EntityTabRepeater} from '../../../Framework/Signum.React/Scripts/Lines'
import { SubTokensOptions, QueryToken, QueryTokenType, hasAnyOrAll }  from '../../../Framework/Signum.React/Scripts/FindOptions'
import { SearchControl }  from '../../../Framework/Signum.React/Scripts/Search'
import { getToString, getMixin }  from '../../../Framework/Signum.React/Scripts/Signum.Entities'
import { TypeContext, FormGroupStyle } from '../../../Framework/Signum.React/Scripts/TypeContext'
import { TemplateTokenMessage } from './Signum.Entities.Templating'
import { EmailTemplateViewMessage } from './Signum.Entities.Mailing'

import QueryTokenEntityBuilder from '../UserAssets/Templates/QueryTokenEntityBuilder'
import QueryTokenBuilder from '../../../Framework/Signum.React/Scripts/SearchControl/QueryTokenBuilder'



export default class TemplateControls extends React.Component<{ queryKey: string; onInsert: (newCode: string)=> void; forHtml: boolean }, {currentToken: QueryToken}>{
    
    state = { currentToken: null };

    render(){
        var ct = this.state.currentToken;

        if(!this.props.queryKey)
            return null;

        return (
            <div className="sf-template-message-insert-container">
                <QueryTokenBuilder queryToken={ct} queryKey={this.props.queryKey} onTokenChange={t=>this.setState({currentToken : t})} subTokenOptions={SubTokensOptions.CanAnyAll | SubTokensOptions.CanElement} readOnly={false} />
                {this.renderButton(EmailTemplateViewMessage.Insert.niceToString(), this.canElement(), token=> `@[${token}]`)  }
                {this.renderButton("if", this.canIf(), token=> this.props.forHtml? 
`<!--@if[${token}]--> <!--@else--> <!--@endif-->`: 
`@if[${token}] @else @endif`)  }
                {this.renderButton("foreach", this.canForeach(), token=> this.props.forHtml? 
`<!--@foreach[${token}]--> <!--@endforeach-->`:
`@foreach[${token}] @endforeach`)  }
                {this.renderButton("endforeach",this.canElement(), token=> this.props.forHtml?
`<!--@any[${token}]--> <!--@notany--> <!--@endany-->`: 
`@any[${token}] @notany @end`)  }
              
            </div>
        );
    }

    renderButton(text: string, canClick: string, buildPattern: (key: string) => string) {
        return <input type="button" disabled={!!canClick} className="btn btn-default btn-sm sf-button" title={canClick} value={text}/>;
    }


    canElement() : string
    {
        var token = this.state.currentToken;

        if (token == null)
            return TemplateTokenMessage.NoColumnSelected.niceToString();

        if (token.type.isCollection)
            return TemplateTokenMessage.YouCannotAddIfBlocksOnCollectionFields.niceToString();
        
        if (hasAnyOrAll(token))
            return TemplateTokenMessage.YouCannotAddBlocksWithAllOrAny.niceToString();

        return null;
    }


    canIf() : string
    {
        var token = this.state.currentToken;

        if (token == null)
            return TemplateTokenMessage.NoColumnSelected.niceToString();

        if (token.type.isCollection)
            return TemplateTokenMessage.YouCannotAddIfBlocksOnCollectionFields.niceToString();
        
        if (hasAnyOrAll(token))
            return TemplateTokenMessage.YouCannotAddBlocksWithAllOrAny.niceToString();

        return null;
    }

    canForeach() : string
    {
             
        var token = this.state.currentToken;

        if (token == null)
            return TemplateTokenMessage.NoColumnSelected.niceToString();

        if (token.type.isCollection)
            return TemplateTokenMessage.YouHaveToAddTheElementTokenToUseForeachOnCollectionFields.niceToString();

        if (token.key != "Element" || token.parent == null || !token.parent.type.isCollection)
            return TemplateTokenMessage.YouCanOnlyAddForeachBlocksWithCollectionFields.niceToString();

        if (hasAnyOrAll(token))
            return TemplateTokenMessage.YouCannotAddBlocksWithAllOrAny.niceToString();

        return null;
    }

    canAny()
    {
          
        var token = this.state.currentToken;

        if (token == null)
            return TemplateTokenMessage.NoColumnSelected.niceToString();

        if (hasAnyOrAll(token))
            return TemplateTokenMessage.YouCannotAddBlocksWithAllOrAny.niceToString();

        return null;
    }
}




