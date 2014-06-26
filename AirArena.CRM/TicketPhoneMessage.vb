Public Class TicketPhoneMessage : Inherits TicketPhoneMessageBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal ticketMessageId As Integer)
        MyBase.new(ticketMessageId)
    End Sub
    Public Sub New(ByVal ticketMessageId As Integer, ByVal durationInMinutes As Integer, ByVal direction As String)
        Me.TicketMessageId = ticketMessageId
        Me.DurationInMinutes = durationInMinutes
        Me.Direction = direction
        Save()
    End Sub

#Region "Properties"
    Enum Directions
        Incoming
        Outgoing
    End Enum
#End Region
End Class