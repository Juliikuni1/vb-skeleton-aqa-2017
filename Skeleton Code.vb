' Skeleton Program for the AQA A1 Summer 2017 examination
' this code should be used in conjunction with the Preliminary Material
' written by the AQA AS1 Programmer Team
' developed in the VB.Net 2008 environment


Imports System.IO

Module Module1
    Const SOIL As Char = "."
    Const SEED As Char = "S"
    Const PLANT As Char = "P"
    Const ROCKS As Char = "X"
    Const FIELDLENGTH As Integer = 20
    Const FIELDWIDTH As Integer = 35

    Function GetHowLongToRun(ByVal printHelp As Boolean) As Integer
        Dim Years As Integer
        Dim Valid As Boolean = False
        If printHelp = True Then
            Console.WriteLine("Welcome to the Plant Growing Simulation")
            Console.WriteLine()
            Console.WriteLine("You can step through the simulation a year at a time")
            Console.WriteLine("or run the simulation for 0 to 5 years")
            Console.WriteLine("How many years do you want the simulation to run?")
        End If
        Do
            Console.Write("Enter a number between 0 and 5, or -1 for stepping mode: ")
            Try
                Years = Console.ReadLine()
                If Years > -2 And Years < 6 Then
                    Valid = True
                Else
                     Console.WriteLine("Invalid input: {0} is not accepted. You may only enter a value from 0-5 or -1", Years)
                End If
            Catch ex As Exception
                Console.WriteLine("That's not a number! Please try again.")
            End Try
        Loop Until Valid
        Return Years
    End Function

    Function CreateNewField() As Char(,)
        Dim Row As Integer
        Dim Column As Integer
        Dim Field(FIELDLENGTH, FIELDWIDTH) As Char
        For Row = 0 To FIELDLENGTH - 1
            For Column = 0 To FIELDWIDTH - 1
                Field(Row, Column) = SOIL
            Next
        Next
        Row = FIELDLENGTH \ 2
        Column = FIELDWIDTH \ 2
        Field(Row, Column) = SEED
        Return Field
    End Function

    Function ReadFile() As Char(,)
        Dim Row As Integer
        Dim Column As Integer
        Dim Field(FIELDLENGTH, FIELDWIDTH) As Char
        Dim FileName As String
        Dim FieldRow As String
        Dim FileHandle As IO.StreamReader
        Console.Write("Enter file name: ")
        FileName = Console.ReadLine()
        Try
            FileHandle = New IO.StreamReader(FileName)
            For Row = 0 To FIELDLENGTH - 1
                FieldRow = FileHandle.ReadLine
                For Column = 0 To FIELDWIDTH - 1
                    Field(Row, Column) = FieldRow(Column)
                Next
            Next
            FileHandle.Close()
        Catch
            Field = CreateNewField()
        End Try
        Return Field
    End Function

    Function InitialiseField() As Char(,)
        Dim Field(FIELDLENGTH, FIELDWIDTH) As Char
        Dim Response As String
        Console.Write("Do you want to load a file with seed positions? (Y/N): ")
        Response = UCase(Console.ReadLine())
        If Response = "Y" Then
            Field = ReadFile()
        Else
            Field = CreateNewField()
        End If
        Return Field
    End Function

    Sub Display(ByVal Field(,) As Char, ByVal Season As String, ByVal Year As Integer)
        Dim Row As Integer
        Dim Column As Integer
        Console.WriteLine("Season: " & Season & "  Year number: " & Year)
        For Row = 0 To FIELDLENGTH - 1
            For Column = 0 To FIELDWIDTH - 1
                Console.Write(Field(Row, Column))
            Next
            Console.WriteLine("|" & Str(Row).PadLeft(3))
        Next
        Console.WriteLine()
    End Sub

    Sub CountPlants(ByVal Field(,) As Char)
        Dim Row As Integer
        Dim Column As Integer
        Dim NumberOfPlants As Integer
        NumberOfPlants = 0
        For Row = 0 To FIELDLENGTH - 1
            For Column = 0 To FIELDWIDTH - 1
                If Field(Row, Column) = PLANT Then
                    NumberOfPlants += 1
                End If
            Next
        Next
        If NumberOfPlants = 1 Then
            Console.WriteLine("There is 1 plant growing")
        Else
            Console.WriteLine("There are " & NumberOfPlants & " plants growing")
        End If
    End Sub

    Function SimulateSpring(ByVal Field As Char(,)) As Char(,)
        Dim Frost As Boolean
        Dim PlantCount As Integer
        For Row = 0 To FIELDLENGTH - 1
            For Column = 0 To FIELDWIDTH - 1
                If Field(Row, Column) = SEED Then
                    Field(Row, Column) = PLANT
                End If
            Next
        Next
        CountPlants(Field)
        If Int(Rnd() * 2) = 1 Then
            Frost = True
        Else
            Frost = False
        End If
        If Frost Then
            PlantCount = 0
            For Row = 0 To FIELDLENGTH - 1
                For Column = 0 To FIELDWIDTH - 1
                    If Field(Row, Column) = PLANT Then
                        PlantCount += 1
                        If PlantCount Mod 3 = 0 Then
                            Field(Row, Column) = SOIL
                        End If
                    End If
                Next
            Next
            Console.WriteLine("There has been a frost")
            CountPlants(Field)
        End If
        Return Field
    End Function

    Function SimulateSummer(ByVal Field(,) As Char) As Char(,)
        Dim RainFall As Integer
        Dim PlantCount As Integer
        RainFall = Int(Rnd() * 3)
        If RainFall = 0 Then
            PlantCount = 0
            For Row = 0 To FIELDLENGTH - 1
                For Column = 0 To FIELDWIDTH - 1
                    If Field(Row, Column) = PLANT Then
                        PlantCount += 1
                        If PlantCount Mod 2 = 0 Then
                            Field(Row, Column) = SOIL
                        End If
                    End If
                Next
            Next
            Console.WriteLine("There has been a severe drought")
            CountPlants(Field)
        End If
        Return Field
    End Function

    Function SeedLands(ByVal Field(,) As Char, ByVal Row As Integer, ByVal Column As Integer) As Char(,)
        If Row >= 0 And Row < FIELDLENGTH And Column >= 0 And Column < FIELDWIDTH Then
            If Field(Row, Column) = SOIL Then
                Field(Row, Column) = SEED
            End If
        End If
        Return Field
    End Function

    Function SimulateAutumn(ByVal Field(,) As Char) As Char(,)
        For Row = 0 To FIELDLENGTH - 1
            For Column = 0 To FIELDWIDTH - 1
                If Field(Row, Column) = PLANT Then
                    Field = SeedLands(Field, Row - 1, Column - 1)
                    Field = SeedLands(Field, Row - 1, Column)
                    Field = SeedLands(Field, Row - 1, Column + 1)
                    Field = SeedLands(Field, Row, Column - 1)
                    Field = SeedLands(Field, Row, Column + 1)
                    Field = SeedLands(Field, Row + 1, Column - 1)
                    Field = SeedLands(Field, Row + 1, Column)
                    Field = SeedLands(Field, Row + 1, Column + 1)
                End If
            Next
        Next
        Return Field
    End Function

    Function SimulateWinter(ByVal Field As Char(,)) As Char(,)
        For Row = 0 To FIELDLENGTH - 1
            For Column = 0 To FIELDWIDTH - 1
                If Field(Row, Column) = PLANT Then
                    Field(Row, Column) = SOIL
                End If
            Next
        Next
        Return Field
    End Function

    Sub SimulateOneYear(ByVal Field(,) As Char, ByVal Year As Integer)
        Field = SimulateSpring(Field)
        Display(Field, "spring", Year)
        Field = SimulateSummer(Field)
        Display(Field, "summer", Year)
        Field = SimulateAutumn(Field)
        Display(Field, "autumn", Year)
        Field = SimulateWinter(Field)
        Display(Field, "winter", Year)
    End Sub

    Sub Simulation(ByVal Optional printHelp As Boolean = True)
        Dim YearsToRun As Integer
        Dim Continuing As Boolean
        Dim Response As String
        Dim Year As Integer
        Dim Field(FIELDWIDTH, FIELDLENGTH) As Char
        YearsToRun = GetHowLongToRun(printHelp)
        If YearsToRun <> 0 Then
            Field = InitialiseField()
            If YearsToRun >= 1 Then
                For Year = 1 To YearsToRun
                    SimulateOneYear(Field, Year)
                Next
            Else If YearsToRun = -1 Then
                Continuing = True
                Year = 0
                While Continuing
                    Year += 1
                    SimulateOneYear(Field, Year)
                    Console.Write("Press Enter to run simulation for another Year, Input X to stop: ")
                    Response = Console.ReadLine()
                    If Response = "x" Or Response = "X" Then
                        Continuing = False
                    End If
                End While
            Else
                Console.WriteLine("Invalid input: {0} is not accepted. You may only enter a value from 0-5 or -1", YearsToRun)
                Simulation(False)
            End If
            Console.WriteLine("End of Simulation")
        End If
        Console.ReadLine()
    End Sub

    Sub Main()
        Randomize()
        Simulation()
    End Sub
End Module