﻿import * as React from 'react'
import { classes } from '../../../../Framework/Signum.React/Scripts/Globals'
import * as Navigator from '../../../../Framework/Signum.React/Scripts/Navigator'
import * as Finder from '../../../../Framework/Signum.React/Scripts/Finder'
import { FormGroup, FormControlStatic, EntityComponentWithState, EntityComponentProps, ValueLine, ValueLineType, EntityLine, EntityCombo, EntityList, EntityRepeater, EntityFrame, EntityTabRepeater, EntityDetail} from '../../../../Framework/Signum.React/Scripts/Lines'
import { SearchControl }  from '../../../../Framework/Signum.React/Scripts/Search'
import { getToString, Lite, is }  from '../../../../Framework/Signum.React/Scripts/Signum.Entities'
import { TypeContext, FormGroupStyle } from '../../../../Framework/Signum.React/Scripts/TypeContext'
import { SendEmailTaskEntity, EmailTemplateEntity } from '../Signum.Entities.Mailing'

export default class SendEmailTask extends EntityComponentWithState<SendEmailTaskEntity, { type: string }> {

    componentWillMount() {
        this.loadEntity(this.props.ctx.value.emailTemplate);
    }

    componentWillReceiveProps(newProps: EntityComponentProps<SendEmailTaskEntity>) {
        if (!is(this.props.ctx.value.emailTemplate, newProps.ctx.value.emailTemplate))
            this.loadEntity(newProps.ctx.value.emailTemplate);
    }

    loadEntity(lite: Lite<EmailTemplateEntity>) {
        if (lite) {
            Navigator.API.fetchAndForget(lite)
                .then(a => Finder.getQueryDescription(a.query.key))
                .then(qd => this.setState({ type: qd.columns["Entity"].type.name }))
                .done();
        }
        else
            this.setState({ type: null });
    }

    renderEntity() {

        var sc = this.props.ctx;
        var ac = this.props.ctx.subCtx({ formGroupStyle: FormGroupStyle.Basic });

        return (
            <div>
                <ValueLine ctx={sc.subCtx(s => s.name) }  />
                <EntityLine ctx={sc.subCtx(s => s.emailTemplate) }  onChange={e => this.loadEntity(e) }/>
                {this.state.type && <EntityLine ctx={sc.subCtx(s => s.targetsFromUserQuery) } /> }
                {this.state.type && <EntityLine ctx={sc.subCtx(s => s.uniqueTarget) } type={{ isLite: true, name: this.state.type }} /> }
            </div>
        );  
    };
}
