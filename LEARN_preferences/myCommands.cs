// (C) Copyright 2016 by  
//
using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

// This line is not mandatory, but improves loading performances
[assembly: CommandClass(typeof(LEARN_preferences.MyCommands))]
[assembly: CommandClass(typeof(DtgoitiaAutoCADLibrary.AutoCADFunctions))]
[assembly: CommandClass(typeof(LeaderPlacement.LeaderCmds))]
[assembly: CommandClass(typeof(MTextCreationAndJigging.Commands))]

namespace LEARN_preferences
{
    public class MyCommands
    {
        [CommandMethod("qq")]
        public void qq()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // Start transaction
            using (Transaction tra = db.TransactionManager.StartTransaction())
            {
                // Get fence point coordinates
                Point3dCollection point3dCollection = GetPolarFence(new Point3d(10, 10, 0), 10.0, 2);

                // Request for objects to be selected by Fence
                PromptSelectionResult selectionResult = ed.SelectFence(point3dCollection);

                // If the prompt status is OK, objects were selected
                if (selectionResult.Status == PromptStatus.OK)
                {
                    SelectionSet ss = selectionResult.Value;

                    ed.WriteMessage("\nNumber of objects selected: " + ss.Count.ToString());
                }
                else
                {
                    Application.ShowAlertDialog("Nothing selected!");
                }

            }

        }
        public Point3dCollection GetPolarFence(Point3d centre, double distance, int resolution)
        {
            // Return the points at a distance from centre. The resolution is the number of points
            // will be taken in 360º. If resolution=3, 3 points will be returned (with 120º between
            // them.
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double angle = 0;
            Point3dCollection ptList = new Point3dCollection();
            for (int i = 0; i < resolution; i++)
            {
                ptList.Add(PolarPoint3d(centre, angle, distance));
                angle = angle + ((2 * Math.PI) / resolution);
            }
            return ptList;
        }
        public Point3d PolarPoint3d(Point3d pt, double angle, double distance)
        {
            return new Point3d(
                pt.X + distance * Math.Cos(angle),
                pt.Y + distance * Math.Sin(angle),
                pt.Z);
        }

        [CommandMethod("qq2")]
        public void qq2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            string blockName = "aa";

            using (Transaction tra = db.TransactionManager.StartTransaction())
            {
                // Open block for read
                BlockTable blockTable = tra.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                ObjectId blockRecordId = ObjectId.Null;

                if (!blockTable.Has(blockName))
                {
                    ed.WriteMessage("\nBlock \"" + blockName + "\" found in the Block Table");
                }
                else
                {
                    ed.WriteMessage("\nBlock \"" + blockName + "\" not found in the Block Table");
                }

            }
        }
        [CommandMethod("qq1")]
        public void qq1() // This method can have any name
        {
            // Put your command code here
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc != null)
            {
                Editor ed = doc.Editor;
                //int number = Application.DocumentManager.MdiActiveDocument.Editor.GetInteger("\nEnter any positive number: ").Value;
                string number = Application.DocumentManager.MdiActiveDocument.Editor.GetInteger("\nEnter any number: ").StringResult;
                ed.WriteMessage("\nIntroduced number: {0}", number);

                // Exit if the user presses ESC or cancels the command
                //if (ppr.Status == PromptStatus.Cancel) return;


                // Prompt for the end point
                //ppo.Message = "\nEnter the end point of the line: ";
                //ppo.UseBasePoint = true;
                //ppo.BasePoint = ptStart;
                //ppr = doc.Editor.GetPoint(ppo);
                //Point3d ptEnd = ppr.Value;
                //if (ppr.Status == PromptStatus.Cancel) return;
            }
        }
        [CommandMethod("ww1")]
        public void ww1()        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc != null)
            {
                Editor      ed = doc.Editor;
                Database    db = doc.Database;

                //string msg1 = "%%U12.564";
                Point3d pt1 = new Point3d(10, 10, 0);
                string msg2 = "Hola pedorro";

                // Add one text
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    // Open the Block Table (bt) for read
                    BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;

                    // Open the Block Table record Model Space for write
                    BlockTableRecord btr = tr.GetObject( bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite ) as BlockTableRecord;

                    // Create a single-line text object
                    DBText text = new DBText();
                    text.SetDatabaseDefaults();
                    text.Position = pt1;
                    text.Height = 0.5;
                    text.TextString = msg2;

                    // Add created object text to the Block Table record
                    btr.AppendEntity(text);
                    tr.AddNewlyCreatedDBObject(text, true);

                    // Save the changes and finish the transaction
                    tr.Commit();
                }

                //Crea 2 textos diferentes, para que puedas ir haciendo pruebas
                //a coger su texto, para sustituir el commando DT:type_clic_level



                // Print message after test scenario is built
                ed.WriteMessage("Hi!\nHere is your scenario.");
            }
        }

        //// Application Session Command with localized name
        //[CommandMethod("MyGroup", "MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        //public void MySessionCmd() // This method can have any name
        //{
        //    // Put your command code here
        //}



        //// LispFunction is similar to CommandMethod but it creates a lisp 
        //// callable function. Many return types are supported not just string
        //// or integer.
        //[LispFunction("MyLispFunction", "MyLispFunctionLocal")]
        //public int MyLispFunction(ResultBuffer args) // This method can have any name
        //{
        //    // Put your command code here

        //    // Return a value to the AutoCAD Lisp Interpreter
        //    return 1;
        //}

    }

}

namespace DtgoitiaAutoCADLibrary
{
    public class ACADAux
    // Class with auxiliary C# functions for AutoCAD
    {
        public static Point3d PolarPoint3d(Point3d pt, double angle, double distance)
        // Equivalent to AutoLISP (polar) function
        {
            return new Point3d(
                pt.X + distance * Math.Cos(angle),
                pt.Y + distance * Math.Sin(angle),
                pt.Z);
        }
        
        public static Point3dCollection GetPolarFence(Point3d centre, double distance, int resolution)
        // Return the points at a distance from centre. The resolution is the number of points
        // will be taken in 360º. If resolution=3, 3 points will be returned (with 120º between
        // them.
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double angle = 0;
            Point3dCollection ptList = new Point3dCollection();
            for (int i = 0; i < resolution; i++)
            {
                ptList.Add(ACADAux.PolarPoint3d(centre, angle, distance));
                angle = angle + ((2 * Math.PI) / resolution);
            }
            return ptList;
        }
    }
    // END public class ACADAux

    
    public class AutoCADFunctions
    // Class with Commands and AutoLISP functions for AutoCAD
    {
        [LispFunction("SelectByDistance")]
        public SelectionSet SelectByDistance(ResultBuffer args)
        // Return a selection set with objects at the specified distance from the specified point
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Point3d centre;
            double distance;
            int resolution;

            string errorStart = "\n >> SelectByDistance error: ";
            if (args == null)
            {
                // If no argument is passed, convert arguments to TypedValue array
                ed.WriteMessage(errorStart + "3 arguments expected.\n");
                return null;
            }
            else
            {
                // If any argument is passed, convert arguments to TypedValue array
                TypedValue[] values = args.AsArray();

                // If the number of arguments is not 3, print error message in command line and return nil
                if (values.Length != 3)
                {
                    ed.WriteMessage(errorStart + "wrong number of arguments.\n");
                    return null;
                }
                // If argument 1 is not a Point3d (list of 3 elements in AutoLISP), print error message in command line and return nil
                else if (values[0].TypeCode != (int)LispDataType.Point3d)
                {
                    ed.WriteMessage(errorStart + "bad type argument 1, point needed.\n");
                    return null;
                }
                // If argument 2 is not a Double (real in AutoLISP), print error message in command line and return nil
                else if (values[1].TypeCode != (int)LispDataType.Double)
                {
                    ed.WriteMessage(errorStart + "bad type argument 2, real needed.\n");
                    return null;
                }
                // If argument 3 is not an integer, print error message in command line and return nil
                else if (values[2].TypeCode != (int)LispDataType.Int16)
                {
                    ed.WriteMessage(errorStart + "bad type argument 3, integer needed.\n");
                    return null;
                }
                // If all arguments are correct, select close elements and return selection set
                else
                {
                    // Pass argument values to variables
                    centre = (Point3d)values[0].Value;
                    distance = (double)values[1].Value;
                    resolution = (int)(Int16)values[2].Value;

                    // Get polygon points
                    Point3dCollection polygonPoint3dCollection = ACADAux.GetPolarFence(centre, distance, resolution);

                    // Select objects "by CrossingPolygon" (fence points taken from polygonPoint3dCollection)
                    PromptSelectionResult selectionResult = ed.SelectCrossingPolygon(polygonPoint3dCollection);

                    if (selectionResult.Status == PromptStatus.OK)
                    {// if any object selected

                        SelectionSet ss = selectionResult.Value;
                        ed.WriteMessage("\n" + ss.Count.ToString() + " objects selected.\n");
                        return ss;
                    }
                    else
                    {// if no objects selected
                        return null;
                    }
                }
            }
        }
        // END SelectByDistance AutoLISP function

        [LispFunction("Convert3DpolyTo2Dpoly")]
        public bool Convert3DpolyTo2Dpoly() // (ResultBuffer args)
        // Convert 3D polyline into 2D polyline. Returns T if succesful, nil if not.
        {
            Application.ShowAlertDialog("it works!");
            return true;
        }
        // END 3DpolyTo2Dpoly AutoLISP function
       

    [CommandMethod("c3p")]// Convert 3d polyline to 2d polyline
        static public void Create3dPolyline()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {
                // Get blocktable and modelspace (for write)
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);

                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                // Create a 3D polyline with 6 segments (closed)
                Point3d[] pts = new Point3d[]
                        { new Point3d(0,0,0),
                          new Point3d(60,0,0),
                         new Point3d(60,0,60),
                         new Point3d(60,60,60),
                         new Point3d(0,60,60),
                         new Point3d(0,0,60)
                        };
                Point3dCollection points = new Point3dCollection(pts);

                Polyline3d poly = new Polyline3d();
                // First add polyline to model space and transaction
                btr.AppendEntity(poly);

                tr.AddNewlyCreatedDBObject(poly, true);
                // Then add all vertices to polyline from point collection
                foreach (Point3d pt in points)
                {
                    // Now create the vertices

                    PolylineVertex3d vex3d = new PolylineVertex3d(pt);

                    // And add them to the 3dpoly (this adds them to the db also)

                    poly.AppendVertex(vex3d);

                    tr.AddNewlyCreatedDBObject(vex3d, true);

                }
                // Make polyline closed
                poly.Closed = true;
                // Change color
                poly.ColorIndex = 14;
                // Commit transaction
                tr.Commit();

            }
        }
    }
    // END public class AutoCADFunctions
    
    
    public class AutoCADCivilFunctions
    // Class with Commands and AutoLISP functions for AutoCAD Civil 3D
    {
        [CommandMethod("TestAutoCADCivil3DCommand")]
        public void TestAutoCADCivil3DCommand()
        {
            Application.ShowAlertDialog("Nothing written yet! xD");
        }
    }
}

namespace LeaderPlacement
{
    public class LeaderCmds
    {
        class DirectionalLeaderJig : EntityJig
        {
            private Point3d _start, _end;
            private string _contents;
            private int _index;
            private int _lineIndex;
            private bool _started;

            public DirectionalLeaderJig(string txt, Point3d start, MLeader ld) : base(ld)
            {
                // Store info that's passed in, but don't init the MLeader
                _contents = txt;
                _start = start;
                _end = start;
                _started = false;
            }

            // A fairly standard Sampler function
            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                var po = new JigPromptPointOptions();
                po.UserInputControls =
                (UserInputControls.Accept3dCoordinates |
                UserInputControls.NoNegativeResponseAccepted);
                po.Message = "\nEnd point";

                var res = prompts.AcquirePoint(po);

                if (_end == res.Value)
                {
                    return SamplerStatus.NoChange;
                }
                else if (res.Status == PromptStatus.OK)
                {
                    _end = res.Value;
                    return SamplerStatus.OK;
                }
                return SamplerStatus.Cancel;
            }
            protected override bool Update()
            {
                var ml = (MLeader)Entity;

                if (!_started)
                {
                    if (_start.DistanceTo(_end) > Tolerance.Global.EqualPoint)
                    {
                        // When the jig actually starts - and we have mouse movement -
                        // we create the MText and init the MLeader
                        ml.ContentType = ContentType.MTextContent;
                        var mt = new MText();
                        mt.Contents = _contents;
                        ml.MText = mt;

                        // Create the MLeader cluster and add a line to it
                        _index = ml.AddLeader();
                        _lineIndex = ml.AddLeaderLine(_index);

                        // Set the vertices on the line                                
                        ml.AddFirstVertex(_lineIndex, _start);
                        ml.AddLastVertex(_lineIndex, _end);

                        // Make sure we don't do this again
                        _started = true;
                    }
                }
                else
                {
                    // We only make the MLeader visible on the second time through
                    // (this also helps avoid some strange geometry flicker)                                                
                    ml.Visible = true;
                    // We already have a line, so just set its last vertex                        
                    ml.SetLastVertex(_lineIndex, _end);
                }
                if (_started)
                {
                    // Set the direction of the text to depend on the X of the end-point
                    // (i.e. is if to the left or right of the start-point?)
                    var dl = new Vector3d((_end.X >= _start.X ? 1 : -1), 0, 0);
                    ml.SetDogleg(_index, dl);
                }
                return true;
            }
        }

        [CommandMethod("dii")]
        public void DirectionalLeader()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var db = doc.Database;

            // Ask the user for the string and the start point of the leader
            var pso = new PromptStringOptions("\nEnter text");
            pso.AllowSpaces = true;
            var pr = ed.GetString(pso);

            if (pr.Status != PromptStatus.OK)
                return;

            var ppr = ed.GetPoint("\nStart point of leader");
            if (ppr.Status != PromptStatus.OK)
                return;

            // Start a transaction, as we'll be jigging a db-resident object
            using (var tr = db.TransactionManager.StartTransaction())
            {
                var bt =
                  (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                var btr =
                  (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite, false);

                // Create and pass in an invisible MLeader
                // This helps avoid flickering when we start the jig
                var ml = new MLeader();
                ml.Visible = false;

                // Create jig
                var jig = new DirectionalLeaderJig(pr.StringResult, ppr.Value, ml);

                // Add the MLeader to the drawing: this allows it to be displayed
                btr.AppendEntity(ml);
                tr.AddNewlyCreatedDBObject(ml, true);

                // Set end point in the jig
                var res = ed.Drag(jig);
                
                // If all is well, commit
                if (res.Status == PromptStatus.OK)
                {
                    tr.Commit();
                }
            }
        }
    }
}

namespace MTextCreationAndJigging
{
    public class Commands
    {
        [CommandMethod("COLTXT2")]
        static public void CreateColouredMText()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Variables for our MText entity's identity and location
            ObjectId mtId;
            Point3d mtLoc = Point3d.Origin;

            int counter = 0;

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                // Create our new MText and set its properties
                MText mt = new MText();
                mt.Location = mtLoc;
                mt.Contents = "Some text in the defaul";

                // Open the block table, the model space and add our MText
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord ms = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                mtId = ms.AppendEntity(mt);
                tr.AddNewlyCreatedDBObject(mt, true);

                // Select the last entity added (our MText)
                PromptSelectionResult psr = ed.SelectLast();

                // Assuming that worked, we'll drag it
                if (psr.Status == PromptStatus.OK)
                {
                    // Launch our drag jig
                    PromptPointResult ppr =
                      ed.Drag(
                        psr.Value,
                        "\nSelect text location: ",
                        delegate (Point3d pt, ref Matrix3d mat)
                        {
                            if (mtLoc == pt)    // If no change has been made, say so
                                return SamplerStatus.NoChange;
                            else    // Otherwise we return the displacement matrix for the current position
                                mat = Matrix3d.Displacement(mtLoc.GetVectorTo(pt));
                            ed.WriteMessage("\n" + counter + "!");
                            counter++;
                            return SamplerStatus.OK;
                        }
                      );
                    
                    // Assuming it works, transform our MText appropriately
                    if (ppr.Status == PromptStatus.OK)
                    {
                        // Get the final translation matrix
                        Matrix3d mat = Matrix3d.Displacement(mtLoc.GetVectorTo(ppr.Value));
                        mt.TransformBy(mat);    // Transform our MText
                        tr.Commit();            // Finally we commit our transaction
                    }
                }
            }
        }

        //[CommandMethod("aa")]
        //static public void DisplayCoordinatesInRealTime()
        //{
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    Database db = doc.Database;
        //    Editor ed = doc.Editor;

        //    int counter = 0;

        //    Jig msg = new Jig();

        //    PromptPointResult ppr = ed.Drag(msg);

        //// NI ZORRA DE COMO SEGUIR
        //    Jig
        //}
    }
}
