Public Class EventType : Inherits EventTypeBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal eventTypeId As Integer)
        MyBase.new(eventTypeId)
    End Sub

#Region "Properties"

    Enum EventTypeName
        StatusChangeUnpaid = 101
        StatusChangePaidProblems
        StatusChangePaid
        StatusChangeInvalidWebsite
        StatusChangePaidPendingURL
        StatusChangePaidPendingApproval
        StatusChangePaidDisapproved
        StatusChangeUnpaidInvalidWebsite
        StatusChangeAccountSetup = 110
        StatusChangeAccountActivated
        StatusChangeInactive = 120
        StatusChangeVoid = 130
        StatusChangePremiumPaid = 150
        SignupCreated = 200
        CustomerPaid
        CustomerTrialExtended = 204
        CustomerSubmitCreditCardFailed = 205
        CustomerSubmitCreditCardFailedCardTypeNotSupported = 206
        ScheduleAdjusted = 207
        CustomerDetailsUpdated = 210
        CustomerUnsubscribed = 220
        CustomerLoggedIn = 230
        CustomerLoggedOut = 240
        CustomerRetrievedAdwrodsKeywordsReport = 250
        AdwordsAccountPaused = 404
        CouldNotPauseAdwords = 406
        FPL_StatusChangetoUnpaid = 501
        FPL_StatusChangetoPaid = 503
        FPL_StatusChangetoActive = 511
        Rehash_NotOffered = 600
        Rehash_Offered = 601
        Rehash_Customer_Took_Up_6_Month_Offer = 602
        Rehash_Customer_Took_Up_12_Month_Offer = 603
        Rehash_CustomerDowngradedAfterFailedPayment = 604


    End Enum

#End Region

    Public Shared Function GetDescription(etn As EventTypeName) As Object
        Return String.Format("{0} - {1}", CInt(etn), etn.ToString.Replace("Rehash_Customer_", String.Empty).Replace("_", " "), etn)
    End Function
    Public Shared Function GetDescription(etn As Integer) As Object
        Return GetDescription(CType(etn, EventTypeName))
    End Function
End Class