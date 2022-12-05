struct Section {
    start: u32,
    end: u32
}

fn first_section_fully_overlaps_second(first: &Section, second: &Section) -> bool {
    first.start <= second.start && first.end >= second.end
}

fn first_section_partially_overlaps_second(first: &Section, second: &Section) -> bool {
    (first.start <= second.start && first.end >= second.start) || (first.start <= second.end && first.end >= second.end)
}

fn sections_fully_overlap(first: &Section, second: &Section) -> bool {
    first_section_fully_overlaps_second(first, second) || first_section_fully_overlaps_second(second, first)
}

fn sections_partially_overlap(first: &Section, second: &Section) -> bool {
    first_section_partially_overlaps_second(first, second) || first_section_partially_overlaps_second(second, first)
}

fn main() {
    let input = std::fs::read_to_string("input.txt").expect("could not read input");

    let mut full_count = 0;
    let mut partial_count = 0;
    for line in input.lines() {
        let mut sections = line.split(',');

        let mut first_range = sections.next().unwrap().split('-');
        let mut second_range = sections.next().unwrap().split('-');

        let first_section = Section {
            start: first_range.next().unwrap().parse().unwrap(),
            end: first_range.next().unwrap().parse().unwrap()
        };
        let second_section = Section {
            start: second_range.next().unwrap().parse().unwrap(),
            end: second_range.next().unwrap().parse().unwrap()
        };

        if sections_fully_overlap(&first_section, &second_section) {
            full_count += 1;
        }

        if sections_partially_overlap(&first_section, &second_section) {
            partial_count += 1;
        }
    }

    println!("{full_count} {partial_count}")
}
