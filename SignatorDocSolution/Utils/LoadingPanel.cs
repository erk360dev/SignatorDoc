
using System;
using System.Drawing;
using System.Windows.Forms;
namespace SignatorDocSolution.Utils
{
    class LoadingPanel: Panel
    {

         //Create a Bitmpap Object.
        //global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSignedBG
        Bitmap animatedImage;
        bool currentlyAnimating = false;

    public LoadingPanel()
    {
        System.IO.Stream stream= new System.IO.MemoryStream();
        global::SignatorDocSolution.Properties.Resources.LoadingImage.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
        animatedImage = new Bitmap(stream);
        this.Size = new Size(150, 150);
        this.BackColor = Color.Transparent;
    }



    //This method begins the animation. 
    public void AnimateImage() 
    {
        if (!currentlyAnimating) 
        {

            //Begin the animation only once.
            ImageAnimator.Animate(animatedImage, new EventHandler(this.OnFrameChanged));
            currentlyAnimating = true;
        }
    }

    private void OnFrameChanged(object o, EventArgs e) 
    {

        //Force a call to the Paint event handler. 
        this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {

        //Begin the animation.
        AnimateImage();

        //Get the next frame ready for rendering.
        ImageAnimator.UpdateFrames();

        //Draw the next frame in the animation.
        e.Graphics.DrawImage(this.animatedImage, new Point(0, 0));
    }
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
            return cp;
        }
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        //base.OnPaintBackground(e);
    }


    }
}
