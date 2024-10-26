using System.IO.Packaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinaryTree
{
    public class TreeNode
    {
        public char Data;
        public TreeNode Left;
        public TreeNode Right;

        public TreeNode(char data) => Data = data;
    }

    public class BinaryTreeData
    {
        public TreeNode Root;

        // 判断输入的字符串能不能构建
        public bool IsValidInput(string input)
        {
            return Regex.IsMatch(input, @"^[A-Z#]+$");
        }

        // 构建二叉树，支持“#”表示空节点
        public TreeNode BuildTree(string data, ref int index)
        {
            if (index >= data.Length || data[index] == '#')
            {
                index++;
                return null;
            }

            TreeNode node = new TreeNode(data[index++]);
            node.Left = BuildTree(data, ref index);
            node.Right = BuildTree(data, ref index);

            return node;
        }

        public void BuildTree(string data)
        {
            int index = 0;
            Root = BuildTree(data, ref index);
        }

        // 递归遍历
        public string PreOrderRecursive(TreeNode node)
        {
            if (node == null) return string.Empty;
            string result = node.Data + " ";
            result += PreOrderRecursive(node.Left);
            result += PreOrderRecursive(node.Right);
            return result;
        }


        public string InOrderRecursive(TreeNode node)
        {
            if (node == null) return string.Empty;
            string result = string.Empty;
            result += InOrderRecursive(node.Left);
            result += node.Data + " ";
            result += InOrderRecursive(node.Right);
            return result;
        }

        public string PostOrderRecursive(TreeNode node)
        {
            if (node == null) return string.Empty;
            string result = string.Empty;
            result += PostOrderRecursive(node.Left);
            result += PostOrderRecursive(node.Right);
            result += node.Data + " ";
            return result;
        }

        // 非递归遍历
        public string PreOrderIterative(TreeNode node)
        {
            if (node == null) return string.Empty;

            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(node);
            string result = string.Empty;
            while (stack.Count > 0)
            {
                TreeNode current = stack.Pop();
                result += current.Data + " ";

                if (current.Right != null) stack.Push(current.Right);
                if (current.Left != null) stack.Push(current.Left);
            }
            return result;
        }

        public string InOrderIterative(TreeNode node)
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            TreeNode current = node;
            string result = string.Empty;
            while (stack.Count > 0 || current != null)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }
                else
                {
                    current = stack.Pop();
                    result += current.Data + " ";
                    current = current.Right;
                }
            }
            return result;
        }

        public string PostOrderIterative(TreeNode node)
        {
            if (node == null) return string.Empty;

            Stack<TreeNode> stack = new Stack<TreeNode>();
            Stack<TreeNode> output = new Stack<TreeNode>();
            string result = string.Empty;
            stack.Push(node);

            while (stack.Count > 0)
            {
                TreeNode current = stack.Pop();
                output.Push(current);

                if (current.Left != null) stack.Push(current.Left);
                if (current.Right != null) stack.Push(current.Right);
            }

            while (output.Count > 0)
            {
                result += output.Pop().Data + " ";
            }
            return result;
        }

        // 层次遍历
        public string LevelOrder(TreeNode node)
        {
            if (node == null) return string.Empty;

            Queue<TreeNode> queue = new Queue<TreeNode>();
            string result = string.Empty;
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                TreeNode current = queue.Dequeue();
                result += current.Data + " ";

                if (current.Left != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
            return result;
        }

        // 统计树的叶子结点
        public int CountLeafNodes(TreeNode node)
        {
            if (node == null) return 0;
            if (node.Left == null && node.Right == null) return 1;
            return CountLeafNodes(node.Left) + CountLeafNodes(node.Right);
        }

        // 使用 WPF 绘制二叉树
        public void DrawTree(Canvas canvas)
        {
            canvas.Children.Clear();
            if (Root == null) return;
            DrawNode(canvas, Root, canvas.ActualWidth / 2, 50, canvas.ActualWidth / 4, 50);
        }

        private void DrawNode(Canvas canvas, TreeNode node, double x, double y, double offsetX, double offsetY)
        {
            if (node == null) return;

            // 绘制当前节点
            Ellipse ellipse = new Ellipse
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.LightBlue,
                Stroke = Brushes.DarkBlue,
                StrokeThickness = 2
            };
            TextBlock text = new TextBlock
            {
                Text = node.Data.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(ellipse, x - 15);
            Canvas.SetTop(ellipse, y - 15);
            Canvas.SetLeft(text, x - 8);
            Canvas.SetTop(text, y - 8);

            canvas.Children.Add(ellipse);
            canvas.Children.Add(text);

            // 绘制左子树连接线和节点
            if (node.Left != null)
            {
                Line line = new Line
                {
                    X1 = x,
                    Y1 = y + 15,
                    X2 = x - offsetX,
                    Y2 = y + offsetY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
                DrawNode(canvas, node.Left, x - offsetX, y + offsetY, offsetX / 2, offsetY);
            }

            // 绘制右子树连接线和节点
            if (node.Right != null)
            {
                Line line = new Line
                {
                    X1 = x,
                    Y1 = y + 15,
                    X2 = x + offsetX,
                    Y2 = y + offsetY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
                DrawNode(canvas, node.Right, x + offsetX, y + offsetY, offsetX / 2, offsetY);
            }
        }
    }

    public partial class MainWindow : Window
    {
        BinaryTreeData tree = new BinaryTreeData();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string _order = InputText.Text;
            if (_order == "Recursion-PreOrder")
            {
                OutputText.Text = tree.PreOrderRecursive(tree.Root);
                InputText.Text = string.Empty;
            }
            else if (_order == "Recursion-InOrder")
            {
                OutputText.Text = tree.InOrderRecursive(tree.Root);
                InputText.Text = string.Empty;
            }
            else if (_order == "Recursion-PostOrder")
            {
                OutputText.Text = tree.PostOrderRecursive(tree.Root);
                InputText.Text = string.Empty;
            }
            else if (_order == "Non-Recursion-PreOrder")
            {
                OutputText.Text = tree.PreOrderIterative(tree.Root);
                InputText.Text = string.Empty;
            }
            else if (_order == "Non-Recursion-InOrder")
            {
                OutputText.Text = tree.InOrderIterative(tree.Root);
                InputText.Text = string.Empty;
            }
            else if (_order == "Non-Recursion-PostOrder")
            {
                OutputText.Text = tree.PostOrderIterative(tree.Root);
                InputText.Text = string.Empty;
            }
            else if (_order == "LevelOrder")
            {
                OutputText.Text = tree.LevelOrder(tree.Root);
                InputText.Text = string.Empty;
            }
            else if (_order == "CountLeafNodes")
            {
                OutputText.Text = tree.CountLeafNodes(tree.Root).ToString();
                InputText.Text = string.Empty;
            }
            else if (tree.IsValidInput(_order))
            {
                tree.BuildTree(_order);
                tree.DrawTree(TreeCanvas); // ABC##DE#G##F###
                InputText.Text = string.Empty;
            }
            else
            {
                Close();
            }
        }
    }
}