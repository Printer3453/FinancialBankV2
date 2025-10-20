import type { AiAnswerDto, AskQuestionDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AiService {
  apiName = 'Default';
  

  askQuestion = (input: AskQuestionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AiAnswerDto>({
      method: 'POST',
      url: '/api/app/ai/ask-question',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
