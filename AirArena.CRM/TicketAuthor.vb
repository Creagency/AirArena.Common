Public Class TicketAuthor : Inherits TicketAuthorBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal i_Code As Integer)
        MyBase.new(i_Code)
    End Sub
#Region "Properties"

    Enum Type
        AccountManager = 1
        Customer
        System
        Internal
        AutoResponder
    End Enum

#End Region
End Class
