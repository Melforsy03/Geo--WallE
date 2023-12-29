drawPoly(poly) = let 
	i = 1;
	prev = poly[0];
	while(i < count(poly))
		let
		draw segment(prev, poly[i]);
		prev := poly[i];
		i++;
		in 0;
	
	draw segment(poly[0], prev);
	in 1;
