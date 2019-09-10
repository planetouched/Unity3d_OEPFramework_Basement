using System;
using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.Path;
using Basement.Common;

namespace Basement.BLFramework.Essential.Choices
{
    public class RleSetPathChoice : PathChoice
    {
        private readonly List<object[]> _list = new List<object[]>();
        private readonly int _length;
        
        public RleSetPathChoice(RawNode node, IContext context)
            : base(node, context)
        {
            var rows = node.GetNode("elements").array;
            foreach (var obj in rows)
            {
                var l = (List<object>)obj;
                var arr = new [] { l[0], Convert.ToInt32(l[1]) };
                _list.Add(arr);
            }

            foreach (var pair in _list)
                _length += (int)pair[1];
        }

        public override ModelsPath GetModelPath()
        {
            int pos = random.Range(0, _length);
            int currentPos = 0;
            foreach (object[] pair in _list)
            {
                if (pos >= currentPos && pos < currentPos + (int)pair[1])
                    return PathUtil.GetModelPath(GetContext(), (string)pair[0], random);
                currentPos += (int)pair[1];
            }

            throw new Exception("nothing selected");
        }

        public override T GetDescription<T>()
        {
            int pos = random.Range(0, _length);
            int currentPos = 0;
            foreach (object[] pair in _list)
            {
                if (pos >= currentPos && pos < currentPos + (int)pair[1])
                    return PathUtil.GetDescription<T>(GetContext(), (string)pair[0], random);
                currentPos += (int)pair[1];
            }

            throw new Exception("nothing selected");
        }
    }
}
