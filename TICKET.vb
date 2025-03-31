Imports MySql.Data.MySqlClient

Public Class TICKET
    Public Property stringName As String

    Public Sub New(name As String, amount As String, method As String, reason As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Set the values received from Form1 to labels or other controls
        Labelname.Text = name
        labelamount.Text = amount
        Labelmethod.Text = method
        Labelreason.Text = reason
    End Sub

    Private Sub TICKET_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeConnection()

        ' Start the timer to update date and time
        Timer1.Start()
    End Sub

    ' Handles the action when the "Add Record" button is clicked
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ' Check if any label is empty before proceeding with database insertion
            If String.IsNullOrWhiteSpace(Labelreason.Text) OrElse String.IsNullOrWhiteSpace(Labelmethod.Text) OrElse String.IsNullOrWhiteSpace(labelamount.Text) OrElse String.IsNullOrWhiteSpace(Labeldate.Text) OrElse String.IsNullOrWhiteSpace(Labeltime.Text) Then
                MessageBox.Show("Please ensure all fields (Reason, Method, Amount, Date, and Time) are filled in before submitting.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Prepare the SQL query with parameters to prevent SQL injection
            Dim sql As String = "INSERT INTO ticket (`NAME`, `REASON`, `METHOD`, `AMOUNT`, `DATE`, `TIME`) VALUES (@Name, @Reason, @Method, @Amount, @Date, @Time)"

            ' Create a new command with the query and connection
            Dim cmd As New MySqlCommand(sql, con)

            ' Add parameters to the command to prevent SQL injection
            cmd.Parameters.AddWithValue("@Name", Labelname.Text)
            cmd.Parameters.AddWithValue("@Reason", Labelreason.Text)
            cmd.Parameters.AddWithValue("@Method", Labelmethod.Text)
            cmd.Parameters.AddWithValue("@Amount", labelamount.Text)
            cmd.Parameters.AddWithValue("@Date", Labeldate.Text)
            cmd.Parameters.AddWithValue("@Time", Labeltime.Text)

            ' Try to open the connection to the database
            con.Open()

            ' Execute the command to insert the data into the database
            cmd.ExecuteNonQuery()

            ' Inform the user that the record has been successfully added
            MessageBox.Show("Record successfully added to the database!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear the labels after successfully adding the record
            ClearFields()

        Catch ex As MySqlException
            ' Handle MySQL-specific errors (e.g., connection issues, query issues)
            MessageBox.Show("A database error occurred: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            ' Handle other general exceptions (e.g., runtime exceptions)
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Ensure that the connection is always closed, even if an error occurs
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

    ' Additional functionality for button clicks to manage the form
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Hide()
        Payment1.Show()
    End Sub

    ' Timer to display current date and time
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Labeldate.Text = Date.Now.ToString("dd-MM-yyyy")
        Labeltime.Text = Date.Now.ToString("hh:mm")
    End Sub

    ' Clear the labels after successfully adding a record
    Private Sub ClearFields()
        Labelname.Text = String.Empty
        Labelreason.Text = String.Empty
        Labelmethod.Text = String.Empty
        labelamount.Text = String.Empty
        Labeldate.Text = String.Empty
        Labeltime.Text = String.Empty
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.ExitThread()
    End Sub
End Class
