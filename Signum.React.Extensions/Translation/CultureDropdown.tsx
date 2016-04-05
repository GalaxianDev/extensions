﻿import * as React from 'react'
import { NavDropdown, MenuItem }  from 'react-bootstrap'
import { Route } from 'react-router'
import { Dic } from '../../../Framework/Signum.React/Scripts/Globals';
import { ajaxPost, ajaxGet } from '../../../Framework/Signum.React/Scripts/Services';
import { EntitySettings } from '../../../Framework/Signum.React/Scripts/Navigator'
import { Entity, Lite, is, toLite } from '../../../Framework/Signum.React/Scripts/Signum.Entities'
import * as Navigator from '../../../Framework/Signum.React/Scripts/Navigator'
import { EntityOperationSettings } from '../../../Framework/Signum.React/Scripts/Operations'
import * as Operations from '../../../Framework/Signum.React/Scripts/Operations'
import { requestTypes, setTypes } from '../../../Framework/Signum.React/Scripts/Reflection'
import { CultureInfoEntity } from '../Basics/Signum.Entities.Basics'
import * as TranslationClient from './TranslationClient'

export interface CultureDropdownProps {
    changeJavascriptCulture: (culture: string) => void;
}

export interface CultureDropdownState {
    cultures?: { [name: string]: Lite<CultureInfoEntity> };
    currentCulture?: Lite<CultureInfoEntity>;
}

export default class CultureDropdown extends React.Component<CultureDropdownProps, CultureDropdownState> {

    constructor(props) {
        super(props);
        this.state = {};
    }

    static changeJavascriptCulture: (ci: CultureInfoEntity) => void;

    componentWillMount() {
        TranslationClient.Api.getCurrentCulture()
            .then(ci => {
                this.setState({ currentCulture: toLite(ci) });
                this.props.changeJavascriptCulture(ci.name);     
                return TranslationClient.Api.getCultures();
            })
            .then(cultures => this.setState({ cultures }))
            .done();
    }

    handleSelect = (c: Lite<CultureInfoEntity>) => {

        TranslationClient.Api.setCurrentCulture(c)
            .then(() => requestTypes())
            .then(types => setTypes(types))
            .then(() => {
                this.setState({ currentCulture: c });
                window.location.reload(true);
            })
            .done();
    }

    render() {
        var cultures = this.state.cultures;

        if (cultures == null)
            return null;


        return (
            <NavDropdown id="culture-dropdown" title={this.state.currentCulture.toStr}>
                {
                    Dic.getValues(cultures).map((c, i) =>
                        <MenuItem key={i} selected={is(c, this.state.currentCulture) } onSelect={() => this.handleSelect(c)}>
                            {c.toStr}
                        </MenuItem>)
                }
            </NavDropdown>
        );
    }
}