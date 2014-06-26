Public Class EmailTemplateComponents : Inherits EmailTemplateComponentsBase

    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal emailTemplateLanguageId As Integer)
        MyBase.new(emailTemplateLanguageId)
    End Sub

    Public Sub New(ByVal emailTemplatelnaguage As EmailTemplateLanguage)
        MyBase.New(emailTemplatelnaguage.EmailTemplateLanguageId)
    End Sub

    Public Sub New(ByVal emailTemplateLanguageId As Integer, ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings)
        MyBase.New(emailTemplateLanguageId, emailTemplateComponentSettings)
    End Sub

    ''' <summary>
    ''' Use this constructor to get split test components (etl invariant)
    ''' </summary>
    ''' <param name="emailTemplateComponentSettings"></param>
    ''' <param name="getSplitTestComponents"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings,
                    ByVal getSplitTestComponents As Boolean)
        MyBase.New(emailTemplateComponentSettings, getSplitTestComponents)
    End Sub

    Public Sub New(ByVal emailTemplateComponentSettings As EmailTemplateComponentsSettings)
        MyBase.New(emailTemplateComponentSettings)
    End Sub
#Region " Properties "
#End Region

#Region " Methods "
#End Region

End Class

Public Class EmailTemplateComponentsSettings
    Public Sub New()

    End Sub

    Public GetAllComponents As Boolean = True
    Public GetOpensTrackingPixelComponent As Integer = 0
    Public GetRegularCCPageUrlComponent As Integer = 0
    Public GetClicksConversionsTrackingCCPageUrlComponent As Integer = 0
    Public GetSplitTestCCPageUrlComponent As Integer = 0
    Public GetMultiVariantTestCCPageUrlComponent As Integer = 0
    Public GetSplitTestOpensTrackingPixelComponent As Integer = 0


End Class

