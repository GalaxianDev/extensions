﻿import * as React from 'react'
import { classes } from '@framework/Globals'
import { FormGroup, FormControlReadonly, ValueLine, ValueLineType, EntityLine, EntityCombo, EntityList, EntityRepeater, EntityTable } from '@framework/Lines'
import { SearchControl } from '@framework/Search'
import { TypeContext, FormGroupStyle } from '@framework/TypeContext'
import FileLine from '../../Files/FileLine'
import { PredictorMetricsEmbedded, PredictorEntity } from '../Signum.Entities.MachineLearning'
import * as Finder from '@framework/Finder'
import { getQueryNiceName } from '@framework/Reflection'
import QueryTokenEntityBuilder from '../../UserAssets/Templates/QueryTokenEntityBuilder'
import { QueryTokenEmbedded } from '../../UserAssets/Signum.Entities.UserAssets'
import { QueryFilterEmbedded } from '../../UserQueries/Signum.Entities.UserQueries'
import { QueryDescription, SubTokensOptions } from '@framework/FindOptions'
import { API } from '../PredictorClient';
import FilterBuilderEmbedded from './FilterBuilderEmbedded';
import { TypeReference } from '@framework/Reflection';

export default class PredictorRegressionMetrics extends React.Component<{ ctx: TypeContext<PredictorEntity> }> {

    render() {
        const ctx = this.props.ctx.subCtx({ formGroupStyle: "SrOnly" });


        return (
            <fieldset>
                <legend>Last results</legend>
                <table className="table table-sm" style={{ width: "initial" }}>
                    <thead>
                        <tr>
                            <th></th>
                            <th>Training</th>
                            <th>Validation</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.renderRow(ctx, a => a.loss)}
                        {this.renderRow(ctx, a => a.evaluation)}
                    </tbody>
                </table>
            </fieldset>
        );
    }

    renderRow(ctx: TypeContext<PredictorEntity>, property: (val: PredictorMetricsEmbedded) => number | null | undefined) {
        const ctxT = ctx.subCtx(a => a.resultTraining!);
        const ctxV = ctx.subCtx(a => a.resultValidation!);
        var unit = ctxT.subCtx(property).propertyRoute.member!.unit;

        return (
            <tr>
                <th>{ctxT.niceName(property)}{unit && " (" + unit + ")"}</th>
                <td><ValueLine ctx={ctxT.subCtx(property)} unitText="" /></td>
                <td><ValueLine ctx={ctxV.subCtx(property)} unitText="" /></td>
            </tr>
        );
    }
}
