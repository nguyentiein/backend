import { JsonObject, JsonProperty, Any } from 'json2typescript';

@JsonObject('DataResponse')
export class DataResponse<T = any> {
  @JsonProperty('data', Any, true)
  data: T[] = [];

  @JsonProperty('meta', Any, true)
  meta: any = undefined;

  @JsonProperty('error', Any, true)
  error: any = undefined;
}
