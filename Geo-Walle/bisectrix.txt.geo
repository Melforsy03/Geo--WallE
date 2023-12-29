bisectrix(a, c, b) = let

m = measure(c, a);

b2, _ = intersect(circle(c, m), ray(c, b));

i1, i2, _ = intersect(circle(a, m), circle(b2, m));

in

line(i1, i2);


point p1(135,98);
circle c1(p1,134);
point p2(150,98);
circle c2(p2,89);
color red;
draw(c1);
color blue;
draw(c2);
restore;
sec = intersect(c1,c2);
draw(sec);