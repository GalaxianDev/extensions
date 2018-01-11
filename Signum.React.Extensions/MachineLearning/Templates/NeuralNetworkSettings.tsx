﻿import * as React from 'react'
import { Popover, OverlayTrigger } from 'react-bootstrap';
import { classes } from '../../../../Framework/Signum.React/Scripts/Globals'
import { FormGroup, FormControlStatic, ValueLine, ValueLineType, EntityLine, EntityCombo, EntityList, EntityRepeater, EntityTable, StyleContext, OptionItem, LineBaseProps } from '../../../../Framework/Signum.React/Scripts/Lines'
import { SearchControl, ValueSearchControl } from '../../../../Framework/Signum.React/Scripts/Search'
import { TypeContext, FormGroupStyle } from '../../../../Framework/Signum.React/Scripts/TypeContext'
import FileLine from '../../Files/FileLine'
import { NeuralNetworkSettingsEntity, PredictorEntity, PredictorColumnUsage, PredictorCodificationEntity, NeuralNetworkHidenLayerEmbedded, PredictorAlgorithmSymbol, NeuralNetworkLearner } from '../Signum.Entities.MachineLearning'
import * as Finder from '../../../../Framework/Signum.React/Scripts/Finder'
import { getQueryNiceName } from '../../../../Framework/Signum.React/Scripts/Reflection'
import QueryTokenEntityBuilder from '../../UserAssets/Templates/QueryTokenEntityBuilder'
import { QueryTokenEmbedded } from '../../UserAssets/Signum.Entities.UserAssets'
import { QueryFilterEmbedded } from '../../UserQueries/Signum.Entities.UserQueries'
import { QueryDescription, SubTokensOptions } from '../../../../Framework/Signum.React/Scripts/FindOptions'
import { API } from '../PredictorClient';
import FilterBuilderEmbedded from './FilterBuilderEmbedded';
import { TypeReference } from '../../../../Framework/Signum.React/Scripts/Reflection';
import { is } from '../../../../Framework/Signum.React/Scripts/Signum.Entities';

export default class NeuralNetworkSettings extends React.Component<{ ctx: TypeContext<NeuralNetworkSettingsEntity> }> {

    handlePredictionTypeChanged = () => {
        var nn = this.props.ctx.value;
        if (nn.predictionType == "Classification" || nn.predictionType == "MultiClassification") {
            nn.lossFunction = "CrossEntropyWithSoftmax";
            nn.evalErrorFunction = "ClassificationError";
        } else {
            nn.lossFunction = "SquaredError";
            nn.evalErrorFunction = "SquaredError";
        }
    }

    render() {
        const ctx = this.props.ctx;

        var p = ctx.findParent(PredictorEntity);

        const ctxb = ctx.subCtx({ formGroupStyle: "Basic" })
        const ctx6 = ctx.subCtx({ labelColumns: 8 })

        return (
            <div>
                <h4>{NeuralNetworkSettingsEntity.niceName()}</h4>
                {p.algorithm && <DeviceLine ctx={ctx.subCtx(a => a.device)} algorithm={p.algorithm} />}
                <ValueLine ctx={ctx.subCtx(a => a.predictionType)} onChange={this.handlePredictionTypeChanged} />
                {this.renderCount(ctx, p, "Input")}
                <EntityTable ctx={ctx.subCtx(a => a.hiddenLayers)} columns={EntityTable.typedColumns<NeuralNetworkHidenLayerEmbedded>([
                    { property: a => a.size, headerHtmlAttributes: { style: { width: "33%" } } },
                    { property: a => a.activation, headerHtmlAttributes: { style: { width: "33%" } } },
                    { property: a => a.initializer, headerHtmlAttributes: { style: { width: "33%" } } },
                ])} />
                <div className="form-vertical">
                    <div className="row">
                        <div className="col-sm-4">
                            {this.renderCount(ctxb, p, "Output")}
                        </div>
                        <div className="col-sm-4">
                            <ValueLine ctx={ctxb.subCtx(a => a.outputActivation)} />
                        </div>
                        <div className="col-sm-4">
                            <ValueLine ctx={ctxb.subCtx(a => a.outputInitializer)} />
                        </div>
                    </div>
                </div>
                <hr />
                <div className="row">
                    <div className="col-sm-6">
                        <ValueLine ctx={ctx6.subCtx(a => a.learner)} onChange={this.handleLearnerChange} helpBlock={this.getHelpBlock(ctx.value.learner)} />
                        <ValueLine ctx={ctx6.subCtx(a => a.learningRate)} />
                        <ValueLine ctx={ctx6.subCtx(a => a.learningMomentum)} formGroupHtmlAttributes={hideFor(ctx6, "AdaDelta", "AdaGrad", "SGD")} />
                        {withHelp(<ValueLine ctx={ctx6.subCtx(a => a.learningUnitGain)} formGroupHtmlAttributes={hideFor(ctx6, "AdaDelta", "AdaGrad", "SGD")} />, <p>true makes it stable (Loss = 1)<br/>false diverge (Loss >> 1)</p>)}
                        <ValueLine ctx={ctx6.subCtx(a => a.learningVarianceMomentum)} formGroupHtmlAttributes={hideFor(ctx6, "AdaDelta", "AdaGrad", "SGD", "MomentumSGD")} />
                    </div>
                    <div className="col-sm-6">
                        <ValueLine ctx={ctx6.subCtx(a => a.minibatchSize)} />
                        <ValueLine ctx={ctx6.subCtx(a => a.numMinibatches)} />
                        <ValueLine ctx={ctx6.subCtx(a => a.saveProgressEvery)} />
                        <ValueLine ctx={ctx6.subCtx(a => a.saveValidationProgressEvery)} />
                    </div>
                </div>
            </div>
        );
    }

    getHelpBlock = (learner: NeuralNetworkLearner | undefined) => {
        switch (learner) {
            case "AdaDelta": return "Did not work :S";
            case "AdaGrad": return "";
            case "Adam": return "";
            case "FSAdaGrad": return "";
            case "MomentumSGD": return "";
            case "RMSProp": return "";
            case "SGD": return "";
        }
    }

    //Values found letting a NN work for a night learning y = sin(x * 5), no idea if they work ok for other cases
    handleLearnerChange = () => {
        var nns = this.props.ctx.value;
        switch (nns.learner) {
            case "Adam":
                nns.learningRate = 1;
                nns.learningMomentum = 0.1;
                nns.learningVarianceMomentum = 0.1;
                nns.learningUnitGain = false;
                break;
            case "AdaDelta":
                nns.learningRate = 1;
                nns.learningMomentum = nns.learningVarianceMomentum = nns.learningUnitGain = null;
                break;
            case "AdaGrad":
                nns.learningRate = 0.1;
                nns.learningMomentum = nns.learningVarianceMomentum = nns.learningUnitGain = null;
                break;
            case "FSAdaGrad":
                nns.learningRate = 0.1;
                nns.learningMomentum = 0.01;
                nns.learningVarianceMomentum = 1;
                nns.learningUnitGain = false;
                break;
            case "MomentumSGD":
                nns.learningRate = 0.1;
                nns.learningMomentum = 0.01;
                nns.learningVarianceMomentum = 0.001;
                nns.learningUnitGain = false;
                break;
            case "RMSProp":
                nns.learningRate = 0.1;
                nns.learningMomentum = 0.01;
                nns.learningVarianceMomentum = 1;
                nns.learningUnitGain = false;
                break;
            case "SGD":
                nns.learningRate = 0.1;
                nns.learningMomentum = nns.learningVarianceMomentum = nns.learningUnitGain = null;
                break;
            default:
        }

        this.forceUpdate();
    }

    renderCount(ctx: StyleContext, p: PredictorEntity, usage: PredictorColumnUsage) {
        return (
            <FormGroup ctx={ctx} labelText={PredictorColumnUsage.niceName(usage) + " columns"}>
                {p.state != "Trained" ? <FormControlStatic ctx={ctx}>?</FormControlStatic> : <ValueSearchControl isBadge={true} isLink={true} findOptions={{
                    queryName: PredictorCodificationEntity,
                    parentColumn: "Predictor",
                    parentValue: p,
                    filterOptions: [
                        { columnName: "Usage", value: usage }
                    ]
                }} />}
            </FormGroup>
        );
    }
}

function withHelp(element: React.ReactElement<LineBaseProps>, text: React.ReactNode): React.ReactElement<any> {
    var ctx = element.props.ctx;
    var id = ctx.prefix + "_help";

    var popover = <Popover id={id} title={ctx.niceName()}> {text}</Popover>;

    var label = <OverlayTrigger trigger="hover" placement="bottom" overlay={popover}>
        <span>{ctx.niceName()} <i className="fa fa-question-circle" aria-hidden="true"></i></span>
    </OverlayTrigger>; 

    return React.cloneElement(element, { labelText: label } as LineBaseProps);
}

function hideFor(ctx: TypeContext<NeuralNetworkSettingsEntity>, ...learners: NeuralNetworkLearner[]): React.HTMLAttributes<any> | undefined {
    return ctx.value.learner && learners.contains(ctx.value.learner) ? ({ style: { opacity: 0.5 } }) : undefined;
}

interface DeviceLineProps {
    ctx: TypeContext<string | null | undefined>;
    algorithm: PredictorAlgorithmSymbol;
}

interface DeviceLineState {
    devices?: string[];
}

export class DeviceLine extends React.Component<DeviceLineProps, DeviceLineState> {

    constructor(props: DeviceLineProps) {
        super(props);
        this.state = {};
    }

    componentWillMount() {
        this.loadData(this.props);
    }

    componentWillReceiveProps(newProps: DeviceLineProps) {
        if (!is(newProps.algorithm, this.props.algorithm))
            this.loadData(newProps);
    }

    loadData(props: DeviceLineProps) {
        API.availableDevices(this.props.algorithm)
            .then(devices => this.setState({ devices }))
            .done();
    }

    render() {
        const ctx = this.props.ctx;
        return (
            <ValueLine ctx={ctx} comboBoxItems={(this.state.devices || []).map(a => ({ label: a, value: a }) as OptionItem)} valueLineType={"ComboBox"} valueHtmlAttributes={{ size: 1 }} />
        );
    }
}
