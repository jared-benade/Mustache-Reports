﻿using Mustache.Reports.Boundry;
using Mustache.Reports.Boundry.Report;
using Mustache.Reports.Boundry.Report.Excel;
using TddBuddy.CleanArchitecture.Domain.Messages;
using TddBuddy.CleanArchitecture.Domain.Output;

namespace Mustache.Reports.Domain
{
    public class RenderExcelUseCase : IRenderExcelUseCase
    {
        private readonly IReportGateway _reportGateway;

        public RenderExcelUseCase(IReportGateway reportGateway)
        {
            _reportGateway = reportGateway;
        }

        public void Execute(RenderExcelInput inputInput, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter)
        {
            var result = _reportGateway.CreateExcelReport(inputInput);

            if (result.HasErrors())
            {
                RespondWithErrors(presenter, result);
                return;
            }

            RespondWithFile(inputInput, presenter, result);
        }

        private static void RespondWithFile(RenderExcelInput input, IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedDocummentOutput result)
        {
            var reportMessage = new WordFileOutput(input.ReportName, result.FetchDocumentAsByteArray());

            presenter.Respond(reportMessage);
        }

        private void RespondWithErrors(IRespondWithSuccessOrError<IFileOutput, ErrorOutputMessage> presenter, RenderedDocummentOutput result)
        {
            var errors = new ErrorOutputMessage();
            errors.AddErrors(result.ErrorMessages);
            presenter.Respond(errors);
        }
    }
}