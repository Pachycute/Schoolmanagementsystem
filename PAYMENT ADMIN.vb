Imports MySql.Data.MySqlClient

Public Class PAYMENT_ADMIN
    Dim Table As New DataTable

    Dim cmd As MySqlCommand
    Dim da As MySqlDataAdapter

    ' This method will be called when the form is loaded
    Private Sub TICKET_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Call LoadData to populate the DataGridView
        InitializeConnection()
        LoadData()
    End Sub

    ' Method to load data into DataGridView
    Private Sub LoadData()
        Try
            ' Ensure the connection is open
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
            Dim sql As String = "SELECT * FROM ticket"
            da = New MySqlDataAdapter(sql, con)
            Table = New DataTable()
            da.Fill(Table)
            DataGridView1.DataSource = Table
        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Ensure the connection is closed after the operation
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

    ' Search functionality when text changes in the search box
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        Try
            ' Ensure the connection is open
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
            Dim query As String = "SELECT * FROM ticket WHERE `NAME` LIKE @searchText"
            cmd = New MySqlCommand(query, con)
            cmd.Parameters.AddWithValue("@searchText", "%" & txtsearch.Text & "%")
            da = New MySqlDataAdapter(cmd)
            Table = New DataTable()
            da.Fill(Table)
            DataGridView1.DataSource = Table
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ' Ensure the connection is closed after the operation
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

    ' Delete ticket functionality
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow = DataGridView1.SelectedRows(0)
            Dim name As String = selectedRow.Cells("NAME").Value
            Dim result = MessageBox.Show("Are you sure you want to delete this ticket?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                Try
                    ' Ensure the connection is open
                    If con.State = ConnectionState.Closed Then
                        con.Open()
                    End If
                    Dim sql As String = "DELETE FROM ticket WHERE NAME = @Name"
                    cmd = New MySqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Name", name)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Ticket deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadData()  ' Refresh the data grid after deletion
                Catch ex As MySqlException
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    ' Ensure the connection is closed after the operation
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End If
        Else
            MessageBox.Show("Please select a ticket to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Button to load all ticket data (same as LoadData method)
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        LoadData()  ' Simply call LoadData to refresh the grid
    End Sub

    ' Button to go back to the admin screen
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        Admin.Show()
    End Sub

    ' Minimize the form
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    ' Exit the application
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.ExitThread()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
