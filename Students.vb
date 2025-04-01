Imports MySql.Data.MySqlClient

Public Class Students
    ' Define the MySQL connection object globally
    ' Assuming con is declared globally

    Dim cmd As MySqlCommand
    Dim da As MySqlDataAdapter
    Dim Table As DataTable

    ' Load data into DataGridView when form is loaded
    Private Sub LoadData()
        ' Create SQL query to retrieve data from the students table
        Dim sql As String = "SELECT `REGISTRATION_NUMBER`, `TRACK`, `SECTION`, `STATUS`, `SESSION`, `NAME`, `LRN`, `SEMESTER`, `STRANDS` FROM students"

        ' Use a Using block to ensure the command and connection are disposed of properly
       
            Using da As New MySqlDataAdapter(sql, con)
                Table = New DataTable()
                Try
                    ' Open the connection
                    If con.State = ConnectionState.Closed Then
                        con.Open()
                    End If

                    ' Fill the DataTable with the data from the database
                    da.Fill(Table)

                    ' Bind the DataTable to the DataGridView
                    DataGridView1.DataSource = Table
                Catch ex As MySqlException
                    ' Handle MySQL-specific errors
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    ' Handle other general errors
                    MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        
    End Sub

    ' Delete a row based on the selected REGISTRATION_NUMBER
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Check if a row is selected in the DataGridView
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Get the REGISTRATION NUMBER of the selected row
            Dim regNumber As String = DataGridView1.SelectedRows(0).Cells("REGISTRATION_NUMBER").Value.ToString()

            ' Confirm deletion with the user
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                ' Use a Using block to ensure the command and connection are disposed of properly

                Using cmd As New MySqlCommand("DELETE FROM students WHERE `REGISTRATION_NUMBER` = @regNumber", con)
                        cmd.Parameters.AddWithValue("@regNumber", regNumber)

                        Try
                            ' Open the connection
                            If con.State = ConnectionState.Closed Then
                                con.Open()
                            End If

                            ' Execute the query to delete the row
                            cmd.ExecuteNonQuery()

                            ' Show a success message
                            MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            ' Refresh the DataGridView
                            LoadData()  ' Call the method to refresh the data in DataGridView
                        Catch ex As MySqlException
                            ' Handle MySQL-specific errors
                            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Catch ex As Exception
                            ' Handle other general errors
                            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Using

            End If
        Else
            ' If no row is selected, show an error message
            MessageBox.Show("Please select a row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Search for a student by NAME
    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        ' Use a Using block to ensure the command and connection are disposed of properly
        Using con As New MySqlConnection("server=127.0.0.1;user id=root;port=3306;password=050720;database=philtech")
            Using cmd As New MySqlCommand("SELECT * FROM students WHERE `NAME` LIKE @searchText", con)
                cmd.Parameters.AddWithValue("@searchText", "%" & txtsearch.Text & "%")
                Using da As New MySqlDataAdapter(cmd)
                    Table = New DataTable()
                    Try
                        ' Open the connection
                        If con.State = ConnectionState.Closed Then
                            con.Open()
                        End If

                        ' Fill the DataTable with the filtered data
                        da.Fill(Table)

                        ' Set the DataSource of DataGridView to the filtered DataTable
                        DataGridView1.DataSource = Table
                    Catch ex As MySqlException
                        MessageBox.Show("Error: " & ex.Message)
                    Catch ex As Exception
                        ' Handle other general exceptions
                        MessageBox.Show("An unexpected error occurred: " & ex.Message)
                    End Try
                End Using
            End Using
        End Using
    End Sub

    ' Form load event to load data when the form is opened
    Private Sub Students_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeConnection()
        LoadData()
    End Sub

    ' Other button event handlers
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        Admin.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Application.ExitThread()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
     Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
     LoadData()
 End Sub
End Class
