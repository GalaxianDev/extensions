//////////////////////////////////
//Auto-generated. Do NOT modify!//
//////////////////////////////////

import { MessageKey, QueryKey, Type, EnumType, registerSymbol } from '../../../Framework/Signum.React/Scripts/Reflection'
import * as Entities from '../../../Framework/Signum.React/Scripts/Signum.Entities'
import * as Basics from '../../../Framework/Signum.React/Scripts/Signum.Entities.Basics'
import * as Authorization from '../Authorization/Signum.Entities.Authorization'


export const QueryStringValueEmbedded = new Type<QueryStringValueEmbedded>("QueryStringValueEmbedded");
export interface QueryStringValueEmbedded extends Entities.EmbeddedEntity {
    Type: "QueryStringValueEmbedded";
    key?: string | null;
    value?: string | null;
}

export const RestApiKeyEntity = new Type<RestApiKeyEntity>("RestApiKey");
export interface RestApiKeyEntity extends Entities.Entity {
    Type: "RestApiKey";
    user?: Entities.Lite<Authorization.UserEntity> | null;
    apiKey?: string | null;
}

export module RestApiKeyOperation {
    export const Save : Entities.ExecuteSymbol<RestApiKeyEntity> = registerSymbol("Operation", "RestApiKeyOperation.Save");
}

export const RestLogEntity = new Type<RestLogEntity>("RestLog");
export interface RestLogEntity extends Entities.Entity {
    Type: "RestLog";
    url: string;
    startDate: string;
    requestBody: string;
    queryString: Entities.MList<QueryStringValueEmbedded>;
    user: Entities.Lite<Basics.IUserEntity> | null;
    controller: string;
    action: string;
    exception: Entities.Lite<Basics.ExceptionEntity> | null;
    responseBody: string | null;
    endDate: string;
}

